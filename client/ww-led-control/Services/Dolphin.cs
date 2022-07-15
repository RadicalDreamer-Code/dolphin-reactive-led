namespace ww_led_control.Services
{
    public class Dolphin
    {
        private bool DEBUG = false;

        public bool emulatorIsHooked;
        public string activeGame = "";

        public IReadOnlyCollection<Common.Offset> offsets = Common.Data.WW_Offsets;
        public List<Common.Offset> selectedOffsets = new List<Common.Offset>();

        private SerialManager _serialManager { get; set; }

        public Dolphin(SerialManager serialManager)
        {
            _serialManager = serialManager;
            if (DEBUG)
            {
                emulatorIsHooked = HookEmulator();
            }
        }

        public bool HookEmulator()
        {
            if (emulatorIsHooked)
                return true;

            emulatorIsHooked = Client.Library.Initialize();

            // get game
            // get offsets by game
            if (emulatorIsHooked)
            {
                activeGame = Client.Library.game.name;
                HookEventsByGame();
            }
            
            return emulatorIsHooked;
        }

        public bool UnhookEmulator()
        {
            Client.Library.Stop();
            emulatorIsHooked = false;
            UnhookEventsByGame();
            return false;
        }

        private void HookEventsByGame()
        {
            // TODO: get game info
            Client.Library.OnWindwakerBeat = OnWindwakerBeat;
            Client.Library.OnHealthChanged = OnHealthChanged;
            Client.Library.OnDoorOpen = OnDoorOpen;

        }

        private void UnhookEventsByGame()
        {
            Client.Library.OnWindwakerBeat = delegate { };
            Client.Library.OnHealthChanged = delegate { };
            Client.Library.OnDoorOpen = delegate { };
        }

        private bool IsSelected(Common.OffsetId offsetId)
        {
            return selectedOffsets.Exists(x => x.id == offsetId);
        }

        // wind waker specific events
        private void OnWindwakerBeat()
        {
            if (!IsSelected(Common.OffsetId.WW_ACTIVEWINDWAKERNOTES))
                return;

            // Get Serial Event
            byte[] messageBytes = { (byte) SerialManager.Commands.TURN_ON, 0x00, 0x00, 0x00 };
            _serialManager.WriteMessage(messageBytes);
            NotifyDataChanged(Common.OffsetId.WW_ACTIVEWINDWAKERNOTES);
        }

        private void OnHealthChanged(byte health)
        {
            if (!IsSelected(Common.OffsetId.WW_CURRENTHEALTH))
                return;

            NotifyDataChanged(Common.OffsetId.WW_CURRENTHEALTH);
        }

        private void OnDoorOpen()
        {
            if (!IsSelected(Common.OffsetId.WW_EVENTCONTROL))
                return;

            NotifyDataChanged(Common.OffsetId.WW_EVENTCONTROL);
        }

        public event Action<Common.OffsetId> OnChange;

        private void NotifyDataChanged(Common.OffsetId o) => OnChange?.Invoke(o);
    }
}
