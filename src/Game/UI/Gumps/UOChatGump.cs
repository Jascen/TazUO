﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClassicUO.Game.Managers;
using ClassicUO.Game.UI.Controls;
using ClassicUO.IO.Resources;
using ClassicUO.Network;
using ClassicUO.Renderer;

using Microsoft.Xna.Framework;

namespace ClassicUO.Game.UI.Gumps
{
    class UOChatGump : Gump
    {
        private readonly ScrollArea _area;
        private readonly Label _currentChannelLabel;

        private readonly List<ChannelListItemControl> _channelList = new List<ChannelListItemControl>();

        public UOChatGump() : base(0, 0)
        {
            CanMove = true;
            AcceptMouseInput = true;

            WantUpdateSize = false;
            Width = 345;
            Height = 390;

            Add(new ResizePic(0x0A28)
            {
                Width = Width,
                Height = Height
            });

            int startY = 25;

            Label text = new Label("Channels", false, 0x0386, 345, 2, FontStyle.None, TEXT_ALIGN_TYPE.TS_CENTER)
            {
                Y = startY
            };
            Add(text);

            startY += 40;

            Add(new AlphaBlendControl(0){ X = 64, Y = startY, Width = 220, Height = 200});
            _area = new ScrollArea(64, startY, 220, 200, true)
            {
                ScrollbarBehaviour = ScrollbarBehaviour.ShowAlways
            };

            foreach (var k in UOChatManager.Channels)
            {
                var chan = new ChannelListItemControl(k.Key, 195);
                _area.Add(chan);
                _channelList.Add(chan);
            }

            Add(_area);

            startY = 275;

            text = new Label("Your current channel:", false, 0x0386, 345, 2, FontStyle.None, TEXT_ALIGN_TYPE.TS_CENTER)
            {
                Y = startY
            };
            Add(text);

            startY += 25;

            _currentChannelLabel = new Label(UOChatManager.CurrentChannelName, false, 0x0386, 345, 2, FontStyle.None, TEXT_ALIGN_TYPE.TS_CENTER)
            {
                Y = startY
            };
            Add(_currentChannelLabel);


            startY = 337;

            Button button = new Button(0, 0x0845, 0x0846, 0x0845)
            {
                X = 48,
                Y = startY + 5,
                ButtonAction = ButtonAction.Activate
            };
            Add(button);

            button = new Button(1, 0x0845, 0x0846, 0x0845)
            {
                X = 123,
                Y = startY + 5,
                ButtonAction = ButtonAction.Activate
            };
            Add(button);

            button = new Button(2, 0x0845, 0x0846, 0x0845)
            {
                X = 216,
                Y = startY + 5,
                ButtonAction = ButtonAction.Activate
            };
            Add(button);

            text = new Label("Join", false, 0x0386, 0, 2, FontStyle.None, TEXT_ALIGN_TYPE.TS_LEFT)
            {
                X = 65,
                Y = startY
            };
            Add(text);

            text = new Label("Leave", false, 0x0386, 0, 2, FontStyle.None, TEXT_ALIGN_TYPE.TS_LEFT)
            {
                X = 140,
                Y = startY
            };
            Add(text);

            text = new Label("Create", false, 0x0386, 0, 2, FontStyle.None, TEXT_ALIGN_TYPE.TS_LEFT)
            {
                X = 233,
                Y = startY
            };
            Add(text);
        }

        public override void OnButtonClick(int buttonID)
        {
            switch (buttonID)
            {
                case 0: // join
                    NetClient.Socket.Send(new PChatJoinCommand("General"));
                    break;
                case 1: // leave
                    break;
                case 2: // create
                    break;
            }
        }

        public void UpdateConference()
        {
            if (_currentChannelLabel.Text != UOChatManager.CurrentChannelName)
                _currentChannelLabel.Text = UOChatManager.CurrentChannelName;
        }

        public void Update()
        {
            foreach (ChannelListItemControl control in _channelList)
            {
                control.Dispose();
            }

            _channelList.Clear();

            foreach (var k in UOChatManager.Channels)
            {
                var c = new ChannelListItemControl(k.Key, 195);
                _channelList.Add(c);
                _channelList.Add(c);
            }
        }

        class ChannelListItemControl : Control
        {
            public ChannelListItemControl(string text, int width)
            {
                Text = text;
                Width = width;
                Add(new Label(text, false, 0x49, Width, font: 3)
                {
                    X = 3
                });
            }

            public readonly string Text;

            public override bool Draw(UltimaBatcher2D batcher, int x, int y)
            {
                ResetHueVector();

                if (MouseIsOver)
                {
                    batcher.Draw2D(Textures.GetTexture(Color.Cyan), x, y, Width, Height, ref _hueVector);
                }

                return base.Draw(batcher, x, y);
            }
        }
    }
}
