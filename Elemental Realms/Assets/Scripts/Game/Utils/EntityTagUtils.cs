using Game.Enum;

namespace Game.Utils
{
    public static class EntityTagUtils
    {
        public static bool HasCommon(this EntityTag mask, EntityTag layerToCheck)
        {
            return (mask & layerToCheck) != 0;
        }
    }
}