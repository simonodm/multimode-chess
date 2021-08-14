using ChessCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Chess
{
    /// <summary>
    /// A form which lists possible options and enables the user to choose one of them.
    /// </summary>
    internal class OptionPickerModalForm : Form
    {
        public Option PickedOption;
        private readonly Dictionary<Button, Option> _buttonToOptionMap;

        public OptionPickerModalForm(IEnumerable<Option> options)
        {
            ClientSize = new Size(320, 600);
            Text = "Choose an option";

            _buttonToOptionMap = new Dictionary<Button, Option>();

            int heightOffset = 0;
            foreach (var option in options)
            {
                var button = new Button()
                {
                    Size = new Size(96, 32),
                    Location = new Point((Width - 96) / 2, heightOffset),
                    Text = option.Text
                };
                button.Click += button_Click;

                _buttonToOptionMap[button] = option;
                heightOffset += 48;

                Controls.Add(button);
            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            PickedOption = _buttonToOptionMap[(Button)sender];
            Close();
            DialogResult = DialogResult.OK;
        }
    }
}
