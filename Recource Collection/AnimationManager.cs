using Microsoft.Xna.Framework;
using Recource_Collection;
using System.Collections.Generic;
using System.Linq;


public class AnimationManager
{
    public enum BossAnimations
    {
        idle,
        smashAttack
    }


    private Dictionary<BossAnimations, Animation> animations;
    public BossAnimations current;
    public bool finishedAnimation => animations[current].IsFinished;
    public AnimationManager(Dictionary<BossAnimations, Animation> animations)
    {
        this.animations = animations;
        current = animations.Keys.First();
    }

    public void Play(BossAnimations name)
    {
        if (current != name)
        {
            current = name;
            animations[current].Reset();


        }
    }
    

    public void Update()
    {
        animations[current].Update();
    }

    public Rectangle GetSourceRect(int frameWidth, int frameHeight) => animations[current].GetSourceRectangle(frameWidth, frameHeight);
    public Vector2 GetSize() => animations[current].Size;
}
