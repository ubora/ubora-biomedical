using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Ubora.Domain.Infrastructure.Specifications
{
    public abstract class Specification<TEntity> : ISpecification<TEntity>, IEquatable<Specification<TEntity>>
    {
        public IQueryable<TEntity> SatisfyEntitiesFrom(IQueryable<TEntity> query)
        {
            return query.Where(ToExpression());
        }

        public bool IsSatisfiedBy(TEntity entity)
        {
            return ToExpression().Compile().Invoke(entity);
        }

        internal abstract Expression<Func<TEntity, bool>> ToExpression();

        // Use && for correctness - specifications use lazy (conditional) evaluation internally.
        public static Specification<TEntity> operator &(Specification<TEntity> spec1, Specification<TEntity> spec2)
        {
            return new AndSpecification<TEntity>(spec1, spec2);
        }

        // Use || for correctness - specifications use lazy (conditional) evaluation internally.
        public static Specification<TEntity> operator |(Specification<TEntity> spec1, Specification<TEntity> spec2)
        {
            return new OrSpecification<TEntity>(spec1, spec2);
        }

        public static Specification<TEntity> operator !(Specification<TEntity> spec1)
        {
            return new NotSpecification<TEntity>(spec1);
        }

        public static bool operator true(Specification<TEntity> spec)
        {
            return false; // No Operation - boilerplate for conditional operators.
        }

        public static bool operator false(Specification<TEntity> spec)
        {
            return false; // No Operation - boilerplate for conditional operators.
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            if (obj.GetType() != GetType())
                return false;
            
            // Specifications are equal, if they are of same type and have all same field values (this counts also automatic properties, because all automatic properties have backing fields accessible through reflection).
            return GetInstanceFields(GetType()).All(f => f.GetValue(obj)?.Equals(f.GetValue(this)) ?? (f.GetValue(this) == null));
        }

        private IEnumerable<FieldInfo> GetInstanceFields(Type type)
        {
            return type?.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public) ?? Enumerable.Empty<FieldInfo>();
        }

        public override int GetHashCode()
        {
            var hashes = GetInstanceFields(GetType()).Select(f => f.GetValue(this)?.GetHashCode() ?? 0).ToList();
            hashes.Add(this.GetType().GetHashCode());
            return CombineHashCodes(hashes);
        }

        public bool Equals(Specification<TEntity> other)
        {
            return Equals((object)other);
        }

        public static bool operator ==(Specification<TEntity> left, Specification<TEntity> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Specification<TEntity> left, Specification<TEntity> right)
        {
            return !Equals(left, right);
        }

        public static int CombineHashCodes(IEnumerable<int> hashCodes)
        {
            // This algorithm is stolen (and modified for our needs) from StackOverflows best answer http://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode/263416#263416

            unchecked // Overflow is fine, just wrap
            {
                int hashResult = 17;

                foreach (var hash in hashCodes)
                {
                    hashResult = (hashResult * 486187739) + hash; // It's better to pick a large prime to multiply, they say (apparently 486187739 is good?)
                }
                return hashResult;
            }
        }
    }
}
