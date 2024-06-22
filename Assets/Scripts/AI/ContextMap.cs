using System;
using UnityEngine;

namespace AI
{
    public class ContextMap
    {
        public enum Mode
        {
            Interest,
            Danger,
            Strafe,
        }
        
        public int Resolution { get; private set; } = 16;

        private Vector2[] _vectors;
        private float[] _weights;

        public ContextMap(int resolution=16)
        {
            Resolution = resolution;
            
            _vectors = new Vector2[resolution];
            _weights = new float[resolution];
            
            // 벡터 채우기
            for (int i = 0; i < Resolution; i++)
            {
                float angle = 360f / resolution * i;
                _vectors[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            }
        }

        public void Add(Vector2 dir, Mode mode = Mode.Interest)
        {
            for (int i = 0; i < Resolution; i++)
            {
                float dot = Vector2.Dot(dir, _vectors[i]);
                switch (mode)
                {
                    case Mode.Interest:
                        _weights[i] += 1 + dot;
                        break;
                    case Mode.Danger:
                        _weights[i] += 1 - dot;
                        break;
                    case Mode.Strafe:
                        _weights[i] += 1 - Mathf.Abs(dot);
                        break;
                }
            }
        }

        public Vector2 GetDestination()
        {
            Vector2 destination = Vector2.zero;
            
            for (int i = 0; i < Resolution; i++)
            {
                destination += _vectors[i] * _weights[i];
            }

            return destination.normalized;
        }

        public void Clear()
        {
            Array.Clear(_weights, 0, _weights.Length);
        }
    }
}