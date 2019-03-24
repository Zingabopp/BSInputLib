using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSInputLib
{
    public class ButtonMap
    {
        public string Name;
        private bool _active;
        public bool Active
        {
            get
            {
                return _active;
            }
            set
            {
                if(_active != value)
                {
                    _active = value;
                    ButtonState?.Invoke(_active);
                }
            }
        }
        /// <summary>
        /// Return true if button is pressed, false otherwise.
        /// </summary>
        /// <returns></returns>
        public event Action<bool> ButtonState;
        public ButtonMap(string name)
        {
            Name = name;
        }
    }
}
