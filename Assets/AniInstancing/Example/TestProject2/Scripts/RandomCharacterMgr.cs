using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [Unity] Fix test functionality(RandomCharacters) +++++
public class RandomCharacterMgr : Singleton<RandomCharacterMgr>
{
    private void Update()
    {
        if (!AnimationInstancing.AnimationInstancingMgr.Instance.UseInstancing)
        {
            return;
        }

        var instancingObjList = AnimationInstancing.AnimationInstancingMgr.Instance.AniInstancingObjList;
        var count = instancingObjList != null ? instancingObjList.Count : 0;

        for (int i = 0; i < count; ++i)
        {
            var instancingObj = instancingObjList[i];

            if (instancingObj == null)
            {
                return;
            }

            if (instancingObj.IsPause())
                instancingObj.CrossFade(0, 0.2f);

            var avatarRange = instancingObj.randomCharacterData._AvartarRange;
            var targetPosition = instancingObj.randomCharacterData._TargetPosition;

            if (Vector3.SqrMagnitude(targetPosition - instancingObj.position) > 25)
            {
                Vector3 curentDir = transform.rotation * Vector3.forward;
                Vector3 wantedDir = (targetPosition - instancingObj.position).normalized;
                instancingObj.SetRotation(Quaternion.RotateTowards(instancingObj.localRotation, Quaternion.LookRotation(targetPosition - instancingObj.localPosition), 8.0f));
            }
            else
            {
                instancingObj.PlayAnimation(UnityEngine.Random.Range(0, 2));
                instancingObj.randomCharacterData._TargetPosition = new Vector3(UnityEngine.Random.Range(-avatarRange, avatarRange), 0, UnityEngine.Random.Range(-avatarRange, avatarRange));

                if (Vector3.SqrMagnitude(targetPosition - instancingObj.localPosition) > Mathf.Epsilon)
                {
                    instancingObj.SetRotation(Quaternion.RotateTowards(instancingObj.localRotation, Quaternion.LookRotation(targetPosition - instancingObj.localPosition), 0.1f));
                }
            }
        }
    }

    [Serializable]
    public class Data
    {
        public float _AvartarRange = 25f;
        [NonSerialized] public Vector3 _TargetPosition = Vector3.zero;

        public Data Clone()
        {
            var clone = new Data();
            clone._AvartarRange = _AvartarRange;
            return clone;
        }
    }
}
// +++++