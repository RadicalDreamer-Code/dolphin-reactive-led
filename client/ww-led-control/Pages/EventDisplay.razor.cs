using Microsoft.AspNetCore.Components;
using System.Linq;
using ww_led_control.Services;

namespace ww_led_control.Pages
{
    public partial class EventDisplay: IDisposable
    {
        private bool runningSpinner = false;
        
        [Parameter]
        public Common.Offset offsetData { get; set; }
        [Parameter] 
        public Action<Common.Offset, object> OnSelectEvent { get; set; }
        [Parameter]
        public Action<Common.OffsetId> OnChange { get; set; }
        private bool selected;


        private string ToggleSpinVisibility() => runningSpinner ? "" : "visually-hidden";

        public async void Animate(Common.OffsetId offsetId)
        {
            if (offsetData.id != offsetId)
                return;

            if (runningSpinner)
                return;

            System.Diagnostics.Debug.Print("Animate " + offsetId.ToString());
            runningSpinner = true;
            await InvokeAsync(StateHasChanged);
            await Task.Delay(TimeSpan.FromSeconds(2));
            runningSpinner = false;
            await InvokeAsync(StateHasChanged);
        }

        protected override void OnInitialized()
        {
            // TODO: This is not really good. Move event trigger
            Dolphin.OnChange += Animate;
            selected = Dolphin.selectedOffsets.Exists(x => x.id == offsetData.id);
        }

        public void Dispose()
        {
            Dolphin.OnChange -= Animate;
        }
    }
}
