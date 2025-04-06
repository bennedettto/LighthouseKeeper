using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LighthouseKeeper.GameStates
{
    [Serializable]
    public class AggregateCondition : IConditionNode
    {
        public enum Aggregate
        {
            And,
            Or,
        }

        public Aggregate aggregate;

        [SerializeReference]
        public List<IConditionNode> conditions = new List<IConditionNode>(new IConditionNode[] { new Condition() });


        public bool IsMet()
        {
            return aggregate == Aggregate.And
                       ? conditions.All(node => node.IsMet())
                       : conditions.Any(node => node.IsMet());
        }


        public int GetHash()
            => conditions.Aggregate(0, (current, condition) => current | condition.GetHash());
    }
}