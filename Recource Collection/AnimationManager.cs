using Microsoft.Xna.Framework;
using Recource_Collection;
using System.Collections.Generic;
using System.Linq;


public class AnimationManager
{
    public enum AnimationName
    {
        idle,
        smashAttack
    }


    private Dictionary<AnimationName, Animation> animations;
    private AnimationName current;

    public AnimationManager(Dictionary<AnimationName, Animation> animations)
    {
        this.animations = animations;
        current = animations.Keys.First();
    }

    public void Play(AnimationName name)
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

    public Rectangle GetSourceRect(int frameWidth, int frameHeight)
    {
        return animations[current].GetSourceRectangle(frameWidth, frameHeight);
    }

    public Vector2 GetSize()
    {
        return animations[current].Size;
    }
}
