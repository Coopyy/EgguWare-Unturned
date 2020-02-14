using SDG.Unturned;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EgguWare.Utilities
{
    //full credits to defcon here, 2 large math for miniscule brain
    public class SphereComponent : MonoBehaviour
    {
        public GameObject Sphere;
        public Vector3 LastPos;

        public float Velocity;
        public float SphereRadius;

        public float LastHit;

        public Vector3 SphereScale;

        public List<Coroutine> Coroutines = new List<Coroutine>();

        void Awake()
        {
            Coroutines.Add(StartCoroutine(RecalcSize()));
            Coroutines.Add(StartCoroutine(CalcSphere()));
            Coroutines.Add(StartCoroutine(CalcVelocity()));

            StartCoroutine(Kill());
        }

        IEnumerator Kill()
        {
            while (true)
            {
                yield return new WaitForSeconds(2f);

                if (Time.realtimeSinceStartup - LastHit < 7)
                    continue;

                foreach (Coroutine c in Coroutines)
                    StopCoroutine(c);
                Destroy(this);
            }
        }

        IEnumerator RecalcSize()
        {
            while (true)
            {
                SphereRadius = G.Settings.AimbotOptions.HitboxSize; //new hitbox size

                Destroy(Sphere);

                Sphere = CreateICOSphere.Create("HitSphere", SphereRadius, G.Settings.AimbotOptions.AimpointMultiplier);
                Sphere.layer = 24; //LAYERMASK.AGENT, might need updating in the future
                Sphere.transform.parent = transform;
                Sphere.transform.localPosition = Vector3.zero;
                Sphere.transform.localScale = SphereScale;

                yield return new WaitForSeconds(0.5f);
            }
        }

        IEnumerator CalcVelocity()
        {
            while (true)
            {
                LastPos = transform.position;
                yield return new WaitForSeconds(0.2f);
                Velocity = Vector3.Distance(transform.position, LastPos) * 5;
            }
        }

        IEnumerator CalcSphere()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);

                float radiusBias = Provider.ping * Velocity * 2;
                float sizeBias = (SphereRadius - radiusBias) / SphereRadius;

                if (sizeBias < 0)
                    sizeBias = 0.05f;

                SphereScale = new Vector3(sizeBias, sizeBias, sizeBias);

                if (Sphere)
                    Sphere.transform.localScale = SphereScale;
            }
        }
    }
}
