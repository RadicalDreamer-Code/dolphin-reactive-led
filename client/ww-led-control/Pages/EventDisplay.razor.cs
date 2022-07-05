using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ww_led_control.Pages
{
    public partial class EventDisplay
    {
        private bool runningSpinner = false;
        
        [Parameter]
        public Common.Offset offsetData { get; set; }
        [Parameter] 
        public Action<Common.Offset, object> OnSelectEvent { get; set; }

        [Parameter]
        public Action OnEventChange { get; set; }

        private string ToggleSpinVisibility() => runningSpinner ? "" : "visually-hidden";

        public async void Animate()
        {
            Console.WriteLine("Animate");
            runningSpinner = true;
            await Task.Delay(2000);
            runningSpinner = false;

        }

    }
}
