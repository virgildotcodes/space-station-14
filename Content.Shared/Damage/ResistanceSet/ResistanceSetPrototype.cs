#nullable enable
using System;
using System.Collections.Generic;
using Robust.Shared.IoC;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.Manager.Attributes;
using Robust.Shared.ViewVariables;

namespace Content.Shared.Damage.ResistanceSet
{
    /// <summary>
    ///     Prototype for the BodyPart class.
    /// </summary>
    [Prototype("resistanceSet")]
    [Serializable, NetSerializable]
    public class ResistanceSetPrototype : IPrototype, ISerializationHooks
    {
        [Dependency]
        private readonly IPrototypeManager _prototypeManager = default!;

        [ViewVariables]
        [DataField("coefficients")]
        public Dictionary<DamageType, float> Coefficients { get; } = new();

        [ViewVariables]
        [DataField("flatReductions")]
        public Dictionary<DamageType, int> FlatReductions { get; } = new();

        [ViewVariables]
        public Dictionary<DamageTypePrototype, ResistanceSetSettings> Resistances { get; private set; } = new();

        [ViewVariables]
        [DataField("id", required: true)]
        public string ID { get; } = default!;

        void ISerializationHooks.AfterDeserialization()
        {
            Resistances = new Dictionary<DamageTypePrototype, ResistanceSetSettings>();

            foreach (var damageType in _prototypeManager.EnumeratePrototypes<DamageTypePrototype>())
            {
                Resistances.Add(damageType, new ResistanceSetSettings(Coefficients[damageType], FlatReductions[damageType]));
            }
        }
    }
}
