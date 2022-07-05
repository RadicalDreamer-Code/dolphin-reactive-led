namespace ww_led_control.Services
{
    public class Dolphin
    {
        private bool DEBUG = false;

        // Emulator
        public bool emulatorIsHooked;
        public string activeGame;
        public IReadOnlyCollection<Common.Offset> offsets = Common.Data.Offsets;
        public List<Common.Offset> selectedOffsets = new List<Common.Offset>();

        public Dolphin()
        {
            if (DEBUG)
            {
                emulatorIsHooked = HookEmulator();
            }

        }

        public bool HookEmulator()
        {
            emulatorIsHooked = Client.Library.Initialize();

            // get game
            // get offsets by game
            if (emulatorIsHooked)
            {
                HookEventsByGame();
            }
            
            return emulatorIsHooked;
        }

        private void HookEventsByGame()
        {
            // TODO: get game info
            Client.Library.OnWindwakerBeat = OnWindwakerBeat;
            Client.Library.OnHealthChanged = OnHealthChanged;
            Client.Library.OnDoorOpen = OnDoorOpen;
        }

        // wind waker specific events
        private void OnWindwakerBeat()
        {
            // do something
            NotifyDataChanged(Common.OffsetId.ACTIVEWINDWAKERNOTES);
        }

        private void OnHealthChanged(byte health)
        {
            // do something
            NotifyDataChanged(Common.OffsetId.CURRENTHEALTH);
        }

        private void OnDoorOpen()
        {
            NotifyDataChanged(Common.OffsetId.EVENTCONTROL);
        }

        public event Action<Common.OffsetId> OnChange;

        private void NotifyDataChanged(Common.OffsetId o) => OnChange?.Invoke(o);
    }
}
