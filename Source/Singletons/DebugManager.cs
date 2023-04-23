using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;

namespace Cubicle.Singletons {
    public static class DebugManager {

        // TODO
        public static SpriteFont Font;

        private static World _currentWorld;
        private static GraphicsDeviceManager _card;
        private static GraphicsDevice _graphics;
        private static SpriteBatch _spriteBatch;

        private static List<String> _primaryText;
        private static List<String> _secondaryText;

        private static Color _background = new Color(0, 0, 0, 145);
        private static Color _foreground = Color.White;

        // TODO: Load ttf instead of monogame specific format
        public static SpriteBatch SpriteBatch { get => _spriteBatch; }

        static DebugManager() {
            _primaryText = new List<String>();
            _secondaryText = new List<String>();
        }

        public static void Initialize(GraphicsDeviceManager card, GraphicsDevice graphics) {
            _card = card;
            _graphics = graphics;
            _spriteBatch = new SpriteBatch(graphics);
        }

        public static void SetWorld(World world) {
            _currentWorld = world;
        }

        public static void BeginDraw() {
            _spriteBatch.Begin();
        }

        public static void EndDraw() {
            // TODO: Support multilines
            // this could be done by just splitting the string
            // at "\".

            _primaryText.Insert(0, $"Cubicle {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}");
            _primaryText.Insert(2, $"Entities: {_currentWorld.EntityCount}");
            // TODO: Debug entities

            // Primary debug info
            for (var i = 0; i < _primaryText.Count; i++) {
                var text = _primaryText[i];
                var size = Font.MeasureString(text);
                var y = i * size.Y;

                _spriteBatch.FillRectangle(new RectangleF(0, y, size.X + 8, size.Y), _background);
                _spriteBatch.DrawString(Font, text, new Vector2(4, y + 2), _foreground);
            }

            // Secondary debug info
            for (var i = 0; i < _secondaryText.Count; i++) {
                var text = _secondaryText[i];
                var size = Font.MeasureString(text);
                var y = i * size.Y;
                var x = _graphics.Viewport.Width - size.X;

                _spriteBatch.FillRectangle(new RectangleF(x - 8, y, size.X + 8, size.Y), _background);
                _spriteBatch.DrawString(Font, text, new Vector2(x - 2, y + 2), _foreground);
            }

            _spriteBatch.End();
            _primaryText.Clear();
            _secondaryText.Clear();
        }


        public static void Text(String text, bool secondary = false) {
            if (!secondary)
                _primaryText.Add(text);
            else
                _secondaryText.Add(text);
        }
        public static void Div(bool secondary = false) {
            if (!secondary)
                _primaryText.Add("");
            else
                _secondaryText.Add("");
        }
    }
}
