using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace Sophia.Models;

public interface IEntity {
    internal const DynamicallyAccessedMemberTypes AccessedMembers =
        PublicConstructors | NonPublicConstructors
                           | PublicProperties | NonPublicProperties
                           | PublicFields | NonPublicFields
                           | Interfaces;
}

public interface IEntity<TKey> : IEntity {
    TKey Id { get; set; }
}
