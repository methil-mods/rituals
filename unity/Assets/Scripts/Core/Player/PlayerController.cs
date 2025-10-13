using System.Collections.Generic;
using Framework;
using Framework.Controller;
using UnityEngine;
using UnityEngine.Events;
using World;

namespace Player
{
    public class PlayerController : BaseController<PlayerController>
    {
        [SerializeField]
        public PlayerData playerData;
        
        [SerializeReference, SubclassSelector]
        public List<Updatable<PlayerController>> updatables = new List<Updatable<PlayerController>>();
        
        public void Start()
        {
            foreach (var updatable in updatables) updatable.Start(this);
        }

        public void Update()
        {
            foreach (var updatable in updatables) updatable.Update(this);
        }

        public void OnDrawGizmos()
        {
            foreach (var updatable in updatables) updatable.OnDrawGizmos();
        }

        public void OnDestroy()
        {
            foreach (var updatable in updatables) updatable.OnDestroy();
        }
    }
}