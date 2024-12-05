using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
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

        private static Vector2 _padding = new Vector2(16, 16);
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
            if (_primaryText.Count <= 0) {
                return;

            }
            // TODO: Support multilines
            // this could be done by just splitting the string
            // at "\".

            _primaryText.Insert(0, $"Cubicle {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}");

            // Primary debug info
            float y = 0;
            for (var i = 0; i < _primaryText.Count; i++) {
                var text = _primaryText[i];
                if (text.Length == 0) {
                    y += _padding.Y * 2;
                    continue;
                }

                var size = Font.MeasureString(text);

                _spriteBatch.FillRectangle(new RectangleF(0, y, size.X + _padding.X, size.Y + _padding.Y), _background);
                _spriteBatch.DrawString(Font, text, new Vector2(_padding.X / 2, y + 2 + _padding.Y / 2), _foreground);

                y += size.Y + _padding.Y;
            }

            // Secondary debug info
            y = 0;
            for (var i = 0; i < _secondaryText.Count; i++) {
                var text = _secondaryText[i];
                if (text.Length == 0) {
                    y += _padding.Y * 2;
                    continue;
                }

                var size = Font.MeasureString(text);
                var x = _graphics.Viewport.Width - size.X;

                _spriteBatch.FillRectangle(new RectangleF(x - _padding.X, y, size.X + _padding.X, size.Y + _padding.Y), _background);
                _spriteBatch.DrawString(Font, text, new Vector2(x - _padding.X / 2, y + 2 + _padding.Y / 2), _foreground);
                y += size.Y + _padding.Y;
            }
            _primaryText.Clear();
            _secondaryText.Clear();
            _spriteBatch.End();
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
