﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameSystem.Constant
{
    public class GameConstants : MonoBehaviour
    {
        public static GameConstants Instance;
        private float GRAVITY_SCALE = 1.0f;
        private static float GLOBAL_GRAVITY_SCALE = -9.81f;

        public const int MIN_VOLUME = 0;
        public const int MIN_FOV = 30;
        public const int MIN_LIGHTING = 10;
        public const int MAX_VOLUME = 100;
        public const int MAX_FOV = 180;
        public const int MAX_LIGHTING = 100;
        public const int DEFAULT_VOLUME = 100;
        public const int DEFAULT_LIGHTING = 100;
        public const int DEFAULT_FOV = 70;
        public const float MOVEMENT_SPEED = 8f;

        private void Awake()
        {
            Instance = this;
        }

        public float GetGravityScale()
        {
            return GRAVITY_SCALE;
        }
    
        public float GetGlobalGravityScale()
        {
            return GLOBAL_GRAVITY_SCALE;
        }
    
        public void SetGravityScale(float GRAVITY_SCALE)
        {
            this.GRAVITY_SCALE = GRAVITY_SCALE;
        }
    }
}

