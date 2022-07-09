using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ww_led_control.Shared
{
	partial class NavMenu
	{
        private bool collapseNavMenu = true;
        private string serialIconClass;

        private string NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            serialIconClass = "oi-circle-x serial-icon-color-red";
            serialManager.OnSerialOpen += OnSerialChange;
        }

        private void OnSerialChange(bool open)
        {
            if (!open)
            {
                serialIconClass = "oi-circle-x serial-icon-color-red";
            }
            else
            {
                serialIconClass = "oi-check serial-icon-color-green";
            }

            StateHasChanged();
        }
    }
}
