using Microsoft.AspNetCore.Components;
using ww_led_control.Services;

namespace ww_led_control.Pages
{
    public partial class EventDisplay
    {
        private bool runningSpinner = false;
        
        [Parameter]
        public Common.Offset offsetData { get; set; }
        [Parameter] 
        public Action<Common.Offset, object> OnSelectEvent { get; set; }


        private string ToggleSpinVisibility() => runningSpinner ? "" : "visually-hidden";

        public async void Animate(Common.OffsetId offsetId)
        {
            Console.WriteLine("Animate");
            if (offsetData.id != offsetId)
                return;

            if (runningSpinner)
                return;

            System.Diagnostics.Debug.Print(offsetId.ToString());
            runningSpinner = true;
            await InvokeAsync(StateHasChanged);
            await Task.Delay(TimeSpan.FromSeconds(2));
            runningSpinner = false;
            await InvokeAsync(StateHasChanged);
        }

        protected override void OnInitialized()
        {
            Dolphin.OnChange += Animate;
        }

    }
}
