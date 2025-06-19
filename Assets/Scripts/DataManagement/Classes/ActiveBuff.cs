using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActiveBuff
{
    public BuffConfigSO Config { get; }
    public float RemainingTime { get; private set; }

    public ActiveBuff(BuffConfigSO cfg)
    {
        Config = cfg;
        RemainingTime = cfg.duration;
    }

    // 返回剩余时间
    public float Tick(float deltaTime)
    {
        if (Config.duration <= 0) return float.MaxValue; // 永久 Buff
        RemainingTime -= deltaTime;
        return RemainingTime;
    }
}

