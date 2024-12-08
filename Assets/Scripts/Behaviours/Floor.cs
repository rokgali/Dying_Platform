using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Floor
{
    private List<Platform> _platforms;

    public Floor(List<Platform> platforms)
    {
        _platforms = platforms;
    }

    public void AddPlatform(Platform platform)
    {
        _platforms.Add(platform);
    }

    public void SteppedOnPlatform(Collider player)
    {
    }
}

