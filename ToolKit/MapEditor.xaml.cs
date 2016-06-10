﻿using mapKnight.Core;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Brushes = System.Windows.Media.Brushes;
using Image = System.Windows.Controls.Image;
using Point = System.Windows.Point;

namespace mapKnight.ToolKit {
    /// <summary>
    /// Interaktionslogik für MapEditor.xaml
    /// </summary>
    public partial class MapEditor : UserControl {
        private enum Tool {
            Pen,
            Eraser,
            Filler,
            Pointer
        }

        private List<UIElement> _Menu = new List<UIElement>( ) {
            new MenuItem( ) { Header = "MAP", Items = {
                    new MenuItem() { Header = "NEW" , Icon = App.Current.FindResource("image_map_new") },
                    new MenuItem() { Header = "LOAD" }
                } },
            new ComboBox() { Width = 260, Margin = new Thickness(-6, 0, -6, 0)},
            new MenuItem() { Header = "SHOW LAYER", IsEnabled = false },
            new CheckBox() { IsChecked = true, Margin = new Thickness(-2, 0, -2, 0) },
            new CheckBox() { IsChecked = true, Margin = new Thickness(-2, 0, -2, 0) },
            new CheckBox() { IsChecked = true, Margin = new Thickness(-2, 0, -2, 0) },
            new Separator() { },
            new MenuItem() {Header ="MODIFY LAYER",IsEnabled =false },
            new RadioButton() {IsChecked = false, GroupName="modifylayer", Margin = new Thickness(-2, 0, -2, 0) },
            new RadioButton() {IsChecked = true, GroupName="modifylayer", Margin = new Thickness(-2, 0, -2, 0) },
            new RadioButton() {IsChecked = false, GroupName="modifylayer", Margin = new Thickness(-2, 0, -2, 0) },
            new Separator(),
            new Border() { Child = (Image)App.Current.FindResource("image_map_pen"), BorderBrush = Brushes.DodgerBlue, BorderThickness=  new Thickness(1), Margin = new Thickness(-6, 0, -6, 0), Padding = new Thickness(6, 0, 6, 0) },
            new Border() { Child = (Image)App.Current.FindResource("image_map_eraser"), BorderBrush = Brushes.DodgerBlue, BorderThickness=  new Thickness(0), Margin = new Thickness(-6, 0, -6,0), Padding = new Thickness(6, 0, 6, 0) },
            new Border() { Child = (Image)App.Current.FindResource("image_map_fill"), BorderBrush = Brushes.DodgerBlue, BorderThickness=  new Thickness(0), Margin = new Thickness(-6, 0, -6 ,0), Padding = new Thickness(6, 0, 6, 0) },
            new Border() { Child = (Image)App.Current.FindResource("image_map_pointer"), BorderBrush = Brushes.DodgerBlue, BorderThickness = new Thickness(0), Margin = new Thickness(-6, 0, -6, 0), Padding = new Thickness(6, 0, 6, 0)}
        };

        public List<UIElement> Menu { get { return _Menu; } }

        private Map currentMap { get { return (Map)((ComboBox)_Menu[1]).SelectedItem; } }
        private Tile currentTile { get { return (wrappanel_tiles.SelectedIndex != -1) ? currentMap.Tiles[wrappanel_tiles.SelectedIndex] : currentMap.Tiles[0]; } }
        private int currentMapIndex { get { return (int)((ComboBox)_Menu[1]).SelectedIndex; } }
        private int currentTileIndex { get { return wrappanel_tiles.SelectedIndex; } }
        private int currentLayer = 1;
        private Tool currentTool = Tool.Pen;

        public MapEditor ( ) {
            InitializeComponent( );

            ((MenuItem)((MenuItem)_Menu[0]).Items[0]).Click += create_map_Click;
            ((ComboBox)_Menu[1]).SelectionChanged += CurrentMapChanged;

            ((CheckBox)_Menu[3]).Checked += (sender, e) => { tilemapview.ShowBackground = true; };
            ((CheckBox)_Menu[3]).Unchecked += (sender, e) => { tilemapview.ShowBackground = false; };
            ((CheckBox)_Menu[4]).Checked += (sender, e) => { tilemapview.ShowMiddle = true; };
            ((CheckBox)_Menu[4]).Unchecked += (sender, e) => { tilemapview.ShowMiddle = false; };
            ((CheckBox)_Menu[5]).Checked += (sender, e) => { tilemapview.ShowForeground = true; };
            ((CheckBox)_Menu[5]).Unchecked += (sender, e) => { tilemapview.ShowForeground = false; };

            ((RadioButton)_Menu[8]).Checked += (sender, e) => { currentLayer = 0; };
            ((RadioButton)_Menu[9]).Checked += (sender, e) => { currentLayer = 1; };
            ((RadioButton)_Menu[10]).Checked += (sender, e) => { currentLayer = 2; };

            _Menu[12].MouseDown += (sender, e) => {
                currentTool = Tool.Pen;
                ResetToolBorders( );
                ((Border)_Menu[12]).BorderThickness = new Thickness(1);
            };
            _Menu[13].MouseDown += (sender, e) => {
                currentTool = Tool.Eraser;
                ResetToolBorders( );
                ((Border)_Menu[13]).BorderThickness = new Thickness(1);
            };
            _Menu[14].MouseDown += (sender, e) => {
                currentTool = Tool.Filler;
                ResetToolBorders( );
                ((Border)_Menu[14]).BorderThickness = new Thickness(1);
            };
            _Menu[15].MouseDown += (sender, e) => {
                currentTool = Tool.Pointer;
                ResetToolBorders( );
                ((Border)_Menu[15]).BorderThickness = new Thickness(1);
            };

            App.ProjectChanged += ( ) => { App.Project.MapAdded += Project_MapAdded; Reset( ); };
        }

        private void Reset ( ) {
            ((ComboBox)_Menu[1]).Items.Clear( );
            wrappanel_tiles.Items.Clear( );
            foreach (Map map in App.Project.GetMaps( )) {
                Project_MapAdded(map);
            }
        }

        private void ResetToolBorders ( ) {
            ((Border)_Menu[12]).BorderThickness = new Thickness(0);
            ((Border)_Menu[13]).BorderThickness = new Thickness(0);
            ((Border)_Menu[14]).BorderThickness = new Thickness(0);
            ((Border)_Menu[15]).BorderThickness = new Thickness(0);
        }

        private void Project_MapAdded (Map obj) {
            MemoryStream memoryStream = new MemoryStream( );
            memoryStream.Position = 0;
            new Bitmap(1, 1).Save(memoryStream, ImageFormat.Png);
            BitmapImage emptyImage = new BitmapImage( );
            emptyImage.BeginInit( );
            emptyImage.StreamSource = memoryStream;
            emptyImage.EndInit( );

            if (obj.Tiles.Length == 0) {
                App.Project.AddTexture(obj, "None", emptyImage);
                obj.AddTile(new Tile( ) { Name = "None", Attributes = new Dictionary<TileAttribute, string>( ) });
            }

            ((ComboBox)_Menu[1]).Items.Add(obj);
            ((ComboBox)_Menu[1]).SelectedIndex = ((ComboBox)_Menu[1]).Items.Count - 1;

            UpdateListbox( );
        }

        private void CurrentMapChanged (object sender, SelectionChangedEventArgs e) {
            if (currentMap == null)
                return;

            UpdateListbox( );
            tilemapview.CurrentMap = currentMap;
            // reset scrollbars
            scrollbar_horizontal.Value = 0;
            scrollbar_horizontal.Minimum = 0;
            scrollbar_horizontal.Maximum = currentMap.Width;
            scrollbar_vertical.Value = 0;
            scrollbar_vertical.Minimum = 0;
            scrollbar_vertical.Maximum = currentMap.Height;
        }

        private void create_map_Click (object sender, RoutedEventArgs e) {
            new CreateMapWindow(tilemapview.GraphicsDevice).ShowDialog( );
            UpdateListbox( );
        }


        public void UpdateListbox ( ) {
            wrappanel_tiles.Items.Clear( );
            foreach (Tile tile in currentMap.Tiles) {
                wrappanel_tiles.Items.Add(new ListViewEntry(App.Project.GetMapWPFTextures(currentMap)[tile.Name]));
            }
            if(wrappanel_tiles.HasItems)
            wrappanel_tiles.SelectedIndex = 0;
        }

        private void wrappanel_tiles_DragEnter (object sender, DragEventArgs e) {
            if (currentMap == null)
                return;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.Copy;
        }

        private void wrappanel_tiles_Drop (object sender, DragEventArgs e) {
            if (currentMap == null)
                return;

            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                string[ ] files = (string[ ])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files) {
                    if (Path.GetExtension(file) == ".png") {
                        AddTileWindow addTileDialog = new AddTileWindow(file);
                        if (addTileDialog.ShowDialog( ) ?? false) {
                            if (currentMap.Tiles.Where(t => t.Name == addTileDialog.Created.Item1.Name) != null) {
                                currentMap.AddTile(addTileDialog.Created.Item1);
                                App.Project.AddTexture(currentMap, addTileDialog.Created.Item1.Name, addTileDialog.Created.Item2);
                                wrappanel_tiles.Items.Add(new ListViewEntry(addTileDialog.Created.Item2));
                            } else {
                                MessageBox.Show("Please don't add tiles with the same name twice!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }

        private void tilemapview_MouseDown (object sender, MouseButtonEventArgs e) {
            if (currentMap == null || currentTileIndex == -1)
                return;

            if (e.RightButton == MouseButtonState.Pressed) {
                Point clickedTile;
                if (GetClickedTile(e, out clickedTile)) {
                    currentMap.Data[(int)clickedTile.X, (int)clickedTile.Y, currentLayer] = 0;
                }
            } else if (e.LeftButton == MouseButtonState.Pressed) {
                Point clickedTile;
                if (GetClickedTile(e, out clickedTile)) {
                    switch (currentTool) {
                        case Tool.Eraser:
                            currentMap.Data[(int)clickedTile.X, (int)clickedTile.Y, currentLayer] = 0;
                            break;
                        case Tool.Filler:
                            if (currentMap.Data[(int)clickedTile.X, (int)clickedTile.Y, currentLayer] == currentTileIndex)
                                break;
                            int searching = currentMap.Data[(int)clickedTile.X, (int)clickedTile.Y, currentLayer];
                            int replacing = currentTileIndex;
                            Queue<Point> pointQueue = new Queue<Point>( );
                            pointQueue.Enqueue(clickedTile);
                            while (pointQueue.Count > 0) {
                                Point current = pointQueue.Dequeue( );
                                if (current.X < 0 || current.X >= currentMap.Width || current.Y < 0 || current.Y >= currentMap.Height)
                                    continue;
                                if (currentMap.Data[(int)current.X, (int)current.Y, currentLayer] == searching) {
                                    currentMap.Data[(int)current.X, (int)current.Y, currentLayer] = replacing;

                                    pointQueue.Enqueue(new Point(current.X - 1, current.Y));
                                    pointQueue.Enqueue(new Point(current.X + 1, current.Y));
                                    pointQueue.Enqueue(new Point(current.X, current.Y - 1));
                                    pointQueue.Enqueue(new Point(current.X, current.Y + 1));
                                }
                            }
                            break;
                        case Tool.Pen:
                            currentMap.Data[(int)clickedTile.X, (int)clickedTile.Y, currentLayer] = currentTileIndex;
                            break;
                        case Tool.Pointer:
                            currentMap.SpawnPoint = new Vector2((int)clickedTile.X, (int)clickedTile.Y);
                            break;
                    }
                }
            }

            tilemapview.Update( );
        }

        private void tilemapview_MouseMove (object sender, MouseEventArgs e) {
            if (currentMap == null)
                return;
            UpdateSelectedTile(e);
            if (currentTileIndex == -1)
                return;

            if (e.RightButton == MouseButtonState.Pressed) {
                Point clickedTile;
                if (GetClickedTile(e, out clickedTile)) {
                    currentMap.Data[(int)clickedTile.X, (int)clickedTile.Y, currentLayer] = 0;
                }
            } else if (e.LeftButton == MouseButtonState.Pressed) {
                Point clickedTile;
                if (GetClickedTile(e, out clickedTile)) {
                    switch (currentTool) {
                        case Tool.Eraser:
                            currentMap.Data[(int)clickedTile.X, (int)clickedTile.Y, currentLayer] = 0;
                            break;
                        case Tool.Pen:
                            currentMap.Data[(int)clickedTile.X, (int)clickedTile.Y, currentLayer] = currentTileIndex;
                            break;
                        case Tool.Pointer:
                            currentMap.SpawnPoint = new Vector2((int)clickedTile.X, (int)clickedTile.Y);
                            break;
                    }
                }
            }
            tilemapview.Update( );
        }

        private bool GetClickedTile (MouseEventArgs e, out Point tile) {
            Point positionOnControl = e.GetPosition(tilemapview);
            tile = new Point(
                    positionOnControl.X / tilemapview.TileSize + tilemapview.Offset.X,
                    currentMap.Height - positionOnControl.Y / tilemapview.TileSize - tilemapview.Offset.Y
                );
            return (tile.X >= 0 && tile.X < currentMap.Width && tile.Y >= 0 && tile.Y < currentMap.Height);
        }

        private void scrollbar_horizontal_ValueChanged (object sender, RoutedPropertyChangedEventArgs<double> e) {
            tilemapview.Offset = new Microsoft.Xna.Framework.Point((int)scrollbar_horizontal.Value, tilemapview.Offset.Y);
        }

        private void scrollbar_vertical_ValueChanged (object sender, RoutedPropertyChangedEventArgs<double> e) {
            tilemapview.Offset = new Microsoft.Xna.Framework.Point(tilemapview.Offset.X, (int)scrollbar_vertical.Value);
        }

        private struct ListViewEntry {
            public ListViewEntry (BitmapImage image) {
                Image = image;
            }

            public BitmapImage Image { get; private set; }
        }

        private void tilemapview_MouseLeave (object sender, MouseEventArgs e) {
            tilemapview.CurrentSelection = new Microsoft.Xna.Framework.Point(-1, -1);
        }

        private void tilemapview_MouseEnter (object sender, MouseEventArgs e) {
            if (currentMap != null)
                UpdateSelectedTile(e);
        }

        private void UpdateSelectedTile (MouseEventArgs e) {
            Point positionOnControl = e.GetPosition(tilemapview);
            tilemapview.CurrentSelection = new Microsoft.Xna.Framework.Point(
                (int)Math.Min(positionOnControl.X / tilemapview.TileSize, currentMap.Width - tilemapview.Offset.X - 1),
                (int)Math.Min(positionOnControl.Y / tilemapview.TileSize, currentMap.Height - tilemapview.Offset.Y - 1)
                );
        }

        private void buttonexport_Click (object sender, RoutedEventArgs e) {
            // export current maps tileset
            SaveFileDialog exportDialog = new SaveFileDialog( );
            exportDialog.Filter = "TileTemplate|*.mkttemplate";
            if (exportDialog.ShowDialog( ) ?? false) {
                using (Stream stream = File.OpenWrite(exportDialog.FileName))
                    TileSerializer.Serialize(stream, currentMap.Tiles, App.Project.GetMapXNATextures(currentMap), tilemapview.GraphicsDevice);
            }
        }
    }
}
