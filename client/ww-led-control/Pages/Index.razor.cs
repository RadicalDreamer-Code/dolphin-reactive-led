using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ww_led_control.Pages
{
    public partial class Index
    {
        private string statusText = STATUS_NOT_HOOKED;
        private MarkupString buttonConnectText = (MarkupString)BUTTON_TEXT_CONNECT;

        private bool spinnerHidden = true;
        private bool buttonClassToggle = true;
        private string ToggleSpinVisibility() => spinnerHidden ? "visually-hidden" : "";
        private string ToggleButtonColor() => buttonClassToggle ? "btn-primary" : "btn-danger";

        private bool buttonDisabled = false;

        private List<Common.Offset> offsets = new();


        protected override void OnInitialized()
        {
            base.OnInitialized();

            // set values
            if (Dolphin.emulatorIsHooked)
                SetDolphin();

        }

        void SelectEvent(Common.Offset offset, object checkedValue)
        {
            if ((bool)checkedValue)
            {
                if (!Dolphin.selectedOffsets.Contains(offset))
                {
                    Dolphin.selectedOffsets.Add(offset);
                }
            }
            else
            {
                if (Dolphin.selectedOffsets.Contains(offset))
                {
                    Dolphin.selectedOffsets.Remove(offset);
                }
            }
        }

        private void OnButtonClickConnect()
        {
            if (Dolphin.emulatorIsHooked)
            {
                DisconnectDolphin();
            }
            else
            {
                ConnectDolphin();
            }
        }

        private void ConnectDolphin()
        {
            // Connect Dolphin
            if (!Dolphin.emulatorIsHooked)
            {
                System.Diagnostics.Debug.WriteLine("Connect with dolphin");
                buttonConnectText = (MarkupString)BUTTON_TEXT_CONNECTING;
                spinnerHidden = false;

                if (Dolphin.HookEmulator())
                {
                    Dolphin.OnChange += OnEventChange;
                    SetDolphin();
                }
                else
                {
                    statusText = STATUS_RETRY;
                    SetButtonRetry();
                }
            }
        }

        private void DisconnectDolphin()
        {
            // Unhook Emulator
            Dolphin.UnhookEmulator();
            Dolphin.OnChange -= OnEventChange;
            Dolphin.selectedOffsets.Clear();
            RemoveGameOffsets();

            statusText = STATUS_NOT_HOOKED;
            SetButtonConnect();
        }

        private void SetButtonConnect()
        {
            buttonConnectText = (MarkupString)BUTTON_TEXT_CONNECT;
            buttonClassToggle = true;
            spinnerHidden = true;
        }
        private void SetButtonRetry()
        {
            buttonConnectText = (MarkupString)BUTTON_TEXT_RETRY;
            buttonClassToggle = true;
            spinnerHidden = true;
        }

        private void SetButtonSuccess()
        {
            buttonConnectText = (MarkupString)BUTTON_TEXT_DISCONNECT;
            buttonClassToggle = false;
            spinnerHidden = true;
        }


        private void SetDolphin()
        {
            statusText = $"{STATUS_SUCCESS} (Game: {Dolphin.activeGame})";
            SetButtonSuccess();
            GetGameOffsets();
        }

        private void GetGameOffsets()
        {
            offsets.Clear();
            foreach (var offset in Dolphin.offsets)
            {
                offsets.Add(offset);
            }
        }

        private void RemoveGameOffsets()
        {
            offsets.Clear();
        }

        private void OnEventChange(Common.OffsetId offsetId)
        {
            // System.Diagnostics.Debug.WriteLine("SPORTACUS");
        }
    }
}
