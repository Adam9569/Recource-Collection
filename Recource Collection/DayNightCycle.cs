namespace Recource_Collection
{
    public enum TimeOfDay
    {
        Day,
        Night
    }

    public class DayNightCycle
    {
        public TimeOfDay CurrentTime { get; private set; } = TimeOfDay.Day;
        public int DayCount { get; private set; } = 1;

        private const float PhaseDuration = 60f;
        private float timer = 0f;

        public bool JustTurnedNight { get; private set; }
        public bool JustTurnedDay { get; private set; }

        public void Update(float deltaTime)
        {
            JustTurnedNight = false;
            JustTurnedDay = false;

            timer += deltaTime;

            if (timer >= PhaseDuration)
            {
                timer = 0f;

                if (CurrentTime == TimeOfDay.Day)
                {
                    CurrentTime = TimeOfDay.Night;
                    JustTurnedNight = true;
                }
                else
                {
                    CurrentTime = TimeOfDay.Day;
                    DayCount++;
                    JustTurnedDay = true;
                }
            }
        }
    }
}
