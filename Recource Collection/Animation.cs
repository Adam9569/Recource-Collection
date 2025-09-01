using Microsoft.Xna.Framework;

namespace Recource_Collection
{
    public class Animation
    {
        public int NumFrames { get; private set; }
        public int Interval { get; private set; }
        public int ActiveFrame { get; private set; }
        public int Row { get; set; }
        public Vector2 Size { get; set; }

        public int counter;

        public Animation(int numFrames, int interval,int row , Vector2 size)
        {
            NumFrames = numFrames;
            Interval = interval;
            ActiveFrame = 0;
            counter = 0;
            Row = row;
            Size = size;
        }

        public void Update()
        {
            counter++;
            if (counter >= Interval)
            {
                counter = 0;
                ActiveFrame++;

                if (ActiveFrame >= NumFrames)
                    ActiveFrame = 0;
            }
        }
        public void Reset()
        {
            ActiveFrame = 0;
            counter = 0;
        }

        public Rectangle GetSourceRectangle(int frameWidth, int frameHeight)
        {
            return new Rectangle(ActiveFrame * frameWidth,frameHeight * Row +1, frameWidth, frameHeight);
        }
    }
}
