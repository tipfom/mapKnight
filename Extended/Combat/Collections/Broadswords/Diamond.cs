﻿using mapKnight.Core;
using mapKnight.Core.World;
using mapKnight.Extended.Graphics.Animation;

namespace mapKnight.Extended.Combat.Collections {
    public static partial class Broadswords {
        public static PrimaryWeapon Diamond (Entity owner) {
            return new PrimaryWeapon(
                "Diamond Broadword", 0, 3f, 1000, "weapons/broadswords/diamond", 180,
                new VertexAnimationData( ) {
                    Offsets = new Vector2[ ] {
                            new Vector2(3, 25),     // sword
                            new Vector2(2, 2),       // hand1
                    },
                    Scales = new float[ ] {
                            0.08244598f,
                            0.08244598f,
                    },
                    Animations = new VertexAnimation[ ] {
                            new VertexAnimation( ) {
                                Name = "idle",
                                CanRepeat = true,
                                Frames = new VertexAnimationFrame[ ] {
                                    new VertexAnimationFrame( ) {
                                        Time = 500,
                                        State = new VertexBone[ ] {
                                            new VertexBone( ) {
                                                Rotation = -47f,
                                                Position = new Vector2(-0.6572658f, -0.2385302f),
                                                Texture = "sword"
                                            },
                                            new VertexBone( ) {
                                                Rotation = -4f,
                                                Position = new Vector2(-0.6098006f, -0.2382041f),
                                                Texture = "hand1"
                                            },
                                        }
                                    },
                                    new VertexAnimationFrame( ) {
                                        Time = 800,
                                        State = new VertexBone[ ] {
                                            new VertexBone( ) {
                                                Rotation = -45f,
                                                Position = new Vector2(-0.664499f, -0.2748923f),
                                                Texture = "sword"
                                            },
                                            new VertexBone( ) {
                                                Rotation = 2f,
                                                Position = new Vector2(-0.6098006f, -0.2895388f),
                                                Texture = "hand1"
                                            },
                                        }
                                    },
                                    new VertexAnimationFrame( ) {
                                        Time = 600,
                                        State = new VertexBone[ ] {
                                            new VertexBone( ) {
                                                Rotation = -51f,
                                                Position = new Vector2(-0.6708236f, -0.2460565f),
                                                Texture = "sword"
                                            },
                                            new VertexBone( ) {
                                                Rotation = -11f,
                                                Position = new Vector2(-0.6215282f, -0.2555707f),
                                                Texture = "hand1"
                                            },
                                        }
                                    },
                                },
                            },
                            new VertexAnimation( ) {
                                Name = "walk",
                                CanRepeat = true,
                                Frames = new VertexAnimationFrame[ ] {
                                    new VertexAnimationFrame( ) {
                                        Time = 100,
                                        State = new VertexBone[ ] {
                                            new VertexBone( ) {
                                                Rotation = -47f,
                                                Position = new Vector2(-0.662088f, -0.2363913f),
                                                Texture = "sword"
                                            },
                                            new VertexBone( ) {
                                                Rotation = -4f,
                                                Position = new Vector2(-0.6098006f, -0.2382041f),
                                                Texture = "hand1"
                                            },
                                        }
                                    },
                                    new VertexAnimationFrame( ) {
                                        Time = 100,
                                        State = new VertexBone[ ] {
                                            new VertexBone( ) {
                                                Rotation = -33f,
                                                Position = new Vector2(-0.5424643f, -0.2287672f),
                                                Texture = "sword"
                                            },
                                            new VertexBone( ) {
                                                Rotation = 13f,
                                                Position = new Vector2(-0.4756282f, -0.2273623f),
                                                Texture = "hand1"
                                            },
                                        }
                                    },
                                    new VertexAnimationFrame( ) {
                                        Time = 130,
                                        State = new VertexBone[ ] {
                                            new VertexBone( ) {
                                                Rotation = -41f,
                                                Position = new Vector2(-0.4834763f, -0.2074512f),
                                                Texture = "sword"
                                            },
                                            new VertexBone( ) {
                                                Rotation = -2f,
                                                Position = new Vector2(-0.4172436f, -0.203935f),
                                                Texture = "hand1"
                                            },
                                        }
                                    },
                                    new VertexAnimationFrame( ) {
                                        Time = 130,
                                        State = new VertexBone[ ] {
                                            new VertexBone( ) {
                                                Rotation = -44f,
                                                Position = new Vector2(-0.7872718f, -0.2384659f),
                                                Texture = "sword"
                                            },
                                            new VertexBone( ) {
                                                Rotation = -5f,
                                                Position = new Vector2(-0.7065725f, -0.2381582f),
                                                Texture = "hand1"
                                            },
                                        }
                                    },
                                    new VertexAnimationFrame( ) {
                                        Time = 170,
                                        State = new VertexBone[ ] {
                                            new VertexBone( ) {
                                                Rotation = -49f,
                                                Position = new Vector2(-0.8837147f, -0.2363269f),
                                                Texture = "sword"
                                            },
                                            new VertexBone( ) {
                                                Rotation = -9f,
                                                Position = new Vector2(-0.7909603f, -0.2338803f),
                                                Texture = "hand1"
                                            },
                                        }
                                    },
                                },
                            },
                            new VertexAnimation( ) {
                                Name = "jump",
                                Frames = new VertexAnimationFrame[ ] {
                                    new VertexAnimationFrame( ) {
                                        Time = 75,
                                        State = new VertexBone[ ] {
                                            new VertexBone( ) {
                                                Rotation = -47f,
                                                Position = new Vector2(-0.6572658f, -0.2385302f),
                                                Texture = "sword"
                                            },
                                            new VertexBone( ) {
                                                Rotation = -4f,
                                                Position = new Vector2(-0.6098006f, -0.2382041f),
                                                Texture = "hand1"
                                            },
                                        }
                                    },
                                    new VertexAnimationFrame( ) {
                                        Time = 75,
                                        State = new VertexBone[ ] {
                                            new VertexBone( ) {
                                                Rotation = -51f,
                                                Position = new Vector2(-0.6572658f, -0.2385302f),
                                                Texture = "sword"
                                            },
                                            new VertexBone( ) {
                                                Rotation = -4f,
                                                Position = new Vector2(-0.6098006f, -0.2382041f),
                                                Texture = "hand1"
                                            },
                                        }
                                    },
                                    new VertexAnimationFrame( ) {
                                        Time = 5,
                                        State = new VertexBone[ ] {
                                            new VertexBone( ) {
                                                Rotation = -56f,
                                                Position = new Vector2(-0.6186885f, -0.1444166f),
                                                Texture = "sword"
                                            },
                                            new VertexBone( ) {
                                                Rotation = -9f,
                                                Position = new Vector2(-0.5808676f, -0.1515768f),
                                                Texture = "hand1"
                                            },
                                        }
                                    },
                                },
                            },
                            new VertexAnimation( ) {
                                Name = "fall",
                                CanRepeat = true,
                                Frames = new VertexAnimationFrame[ ] {
                                    new VertexAnimationFrame( ) {
                                        Time = 180,
                                        State = new VertexBone[ ] {
                                            new VertexBone( ) {
                                                Rotation = -56f,
                                                Position = new Vector2(-0.6186885f, -0.1444166f),
                                                Texture = "sword"
                                            },
                                            new VertexBone( ) {
                                                Rotation = -9f,
                                                Position = new Vector2(-0.5808676f, -0.1515768f),
                                                Texture = "hand1"
                                            },
                                        }
                                    },
                                    new VertexAnimationFrame( ) {
                                        Time = 180,
                                        State = new VertexBone[ ] {
                                            new VertexBone( ) {
                                                Rotation = -72f,
                                                Position = new Vector2(-0.8509465f, -0.1163473f),
                                                Texture = "sword"
                                            },
                                            new VertexBone( ) {
                                                Rotation = -33f,
                                                Position = new Vector2(-0.7949829f, -0.1288491f),
                                                Texture = "hand1"
                                            },
                                        }
                                    },
                                    new VertexAnimationFrame( ) {
                                        Time = 180,
                                        State = new VertexBone[ ] {
                                            new VertexBone( ) {
                                                Rotation = -76f,
                                                Position = new Vector2(-0.5130889f, -0.1302504f),
                                                Texture = "sword"
                                            },
                                            new VertexBone( ) {
                                                Rotation = -26f,
                                                Position = new Vector2(-0.4571251f, -0.1542533f),
                                                Texture = "hand1"
                                            },
                                        }
                                    },
                                },
                            },
                            new VertexAnimation( ) {
                                Name = "attack0",
                                Frames = new VertexAnimationFrame[ ] {
                                    new VertexAnimationFrame( ) {
                                        Time = 180,
                                        State = new VertexBone[ ] {
                                            new VertexBone( ) {
                                                Rotation = -47f,
                                                Position = new Vector2(-0.6572658f, -0.2385302f),
                                                Texture = "sword"
                                            },
                                            new VertexBone( ) {
                                                Rotation = -4f,
                                                Position = new Vector2(-0.6098006f, -0.2382041f),
                                                Texture = "hand1"
                                            },
                                        }
                                    },
                                    new VertexAnimationFrame( ) {
                                        Time = 100,
                                        State = new VertexBone[ ] {
                                            new VertexBone( ) {
                                                Rotation = 26f,
                                                Position = new Vector2(-0.3223347f, -0.1126988f),
                                                Texture = "sword"
                                            },
                                            new VertexBone( ) {
                                                Rotation = 75f,
                                                Position = new Vector2(-0.286788f, -0.07521598f),
                                                Texture = "hand1"
                                            },
                                        }
                                    },
                                    new VertexAnimationFrame( ) {
                                        Time = 170,
                                        State = new VertexBone[ ] {
                                            new VertexBone( ) {
                                                Rotation = -56f,
                                                Position = new Vector2(-0.1632493f, -0.158121f),
                                                Texture = "sword"
                                            },
                                            new VertexBone( ) {
                                                Rotation = -12f,
                                                Position = new Vector2(-0.06904832f, -0.1699104f),
                                                Texture = "hand1"
                                            },
                                        }
                                    },
                                    new VertexAnimationFrame( ) {
                                        Time = 1,
                                        State = new VertexBone[ ] {
                                            new VertexBone( ) {
                                                Rotation = -47f,
                                                Position = new Vector2(-0.6572658f, -0.2385302f),
                                                Texture = "sword"
                                            },
                                            new VertexBone( ) {
                                                Rotation = -4f,
                                                Position = new Vector2(-0.6098006f, -0.2382041f),
                                                Texture = "hand1"
                                            },
                                        }
                                    },
                                },
                            },
                            new VertexAnimation( ) {
                                Name = "attack1",
                                Frames = new VertexAnimationFrame[ ] {
                                    new VertexAnimationFrame( ) {
                                        Time = 200,
                                        State = new VertexBone[ ] {
                                            new VertexBone( ) {
                                                Rotation = -47f,
                                                Position = new Vector2(-0.6572658f, -0.2385302f),
                                                Texture = "sword"
                                            },
                                            new VertexBone( ) {
                                                Rotation = -4f,
                                                Position = new Vector2(-0.6098006f, -0.2382041f),
                                                Texture = "hand1"
                                            },
                                        }
                                    },
                                    new VertexAnimationFrame( ) {
                                        Time = 110,
                                        State = new VertexBone[ ] {
                                            new VertexBone( ) {
                                                Rotation = -51f,
                                                Position = new Vector2(0.2253399f, -0.2667516f),
                                                Texture = "sword"
                                            },
                                            new VertexBone( ) {
                                                Rotation = -4f,
                                                Position = new Vector2(0.2194768f, -0.2596855f),
                                                Texture = "hand1"
                                            },
                                        }
                                    },
                                    new VertexAnimationFrame( ) {
                                        Time = 150,
                                        State = new VertexBone[ ] {
                                            new VertexBone( ) {
                                                Rotation = -17f,
                                                Position = new Vector2(0.4678186f, 0.3089487f),
                                                Texture = "sword"
                                            },
                                            new VertexBone( ) {
                                                Rotation = 34f,
                                                Position = new Vector2(0.4716547f, 0.2988298f),
                                                Texture = "hand1"
                                            },
                                        }
                                    },
                                    new VertexAnimationFrame( ) {
                                        Time = 1,
                                        State = new VertexBone[ ] {
                                            new VertexBone( ) {
                                                Rotation = 4f,
                                                Position = new Vector2(-0.5051419f, -0.2405478f),
                                                Texture = "sword"
                                            },
                                            new VertexBone( ) {
                                                Rotation = 61f,
                                                Position = new Vector2(-0.4175698f, -0.2213784f),
                                                Texture = "hand1"
                                            },
                                        }
                                    },
                                },
                            },
                            new VertexAnimation( ) {
                                Name = "attack2",
                                Frames = new VertexAnimationFrame[ ] {
                                    new VertexAnimationFrame( ) {
                                        Time = 200,
                                        State = new VertexBone[ ] {
                                            new VertexBone( ) {
                                                Rotation = 4f,
                                                Position = new Vector2(-0.5051419f, -0.2405478f),
                                                Texture = "sword"
                                            },
                                            new VertexBone( ) {
                                                Rotation = 61f,
                                                Position = new Vector2(-0.4175698f, -0.2213784f),
                                                Texture = "hand1"
                                            },
                                        }
                                    },
                                    new VertexAnimationFrame( ) {
                                        Time = 90,
                                        State = new VertexBone[ ] {
                                            new VertexBone( ) {
                                                Rotation = -46f,
                                                Position = new Vector2(-1.117555f, -0.2106026f),
                                                Texture = "sword"
                                            },
                                            new VertexBone( ) {
                                                Rotation = -17f,
                                                Position = new Vector2(-1.039627f, -0.2085447f),
                                                Texture = "hand1"
                                            },
                                        }
                                    },
                                    new VertexAnimationFrame( ) {
                                        Time = 150,
                                        State = new VertexBone[ ] {
                                            new VertexBone( ) {
                                                Rotation = -50f,
                                                Position = new Vector2(0.4110667f, -0.1250447f),
                                                Texture = "sword"
                                            },
                                            new VertexBone( ) {
                                                Rotation = -3f,
                                                Position = new Vector2(0.4287176f, -0.1326121f),
                                                Texture = "hand1"
                                            },
                                        }
                                    },
                                    new VertexAnimationFrame( ) {
                                        Time = 1,
                                        State = new VertexBone[ ] {
                                            new VertexBone( ) {
                                                Rotation = -47f,
                                                Position = new Vector2(-0.6572658f, -0.2385302f),
                                                Texture = "sword"
                                            },
                                            new VertexBone( ) {
                                                Rotation = -4f,
                                                Position = new Vector2(-0.6098006f, -0.2382041f),
                                                Texture = "hand1"
                                            },
                                        }
                                    },
                                },
                            },
                    }
                },
                new Transform(new Vector2(.4f / owner.Transform.Width, -.1f * owner.Transform.Height), new Vector2(2 * owner.Transform.Width, .8f * owner.Transform.Height)),
                owner);
        }
    }
}
