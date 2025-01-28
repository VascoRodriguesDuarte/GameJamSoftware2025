using UnityEngine;

public static class GameTerrain
{
    public enum MajorType {
        Land,
        Air,
        Solid
    }

    public enum MinorType {
        Enamel,
        Dentin,
        Pulp,
        Gum,
        Bone,
        Air
    }

    public static MajorType GetMajorType(MinorType type) {
        switch (type)
        {
            case MinorType.Enamel:
            return MajorType.Land;
            case MinorType.Dentin:
            return MajorType.Land;
            case MinorType.Pulp:
            return MajorType.Land;
            case MinorType.Gum:
            return MajorType.Land;
            case MinorType.Bone:
            return MajorType.Solid;
            case MinorType.Air:
            return MajorType.Air;
            default:
            Debug.Log("Uh Oh");
            return MajorType.Land;

        }

    }

    public static bool IsDiggable(MinorType type) {
        switch (type)
        {
            case MinorType.Enamel:
            return true;
            case MinorType.Dentin:
            return false;
            case MinorType.Pulp:
            return false;
            case MinorType.Gum:
            return false;
            case MinorType.Bone:
            return false;
            case MinorType.Air:
            return false; 
            default:
            Debug.Log("Uh Oh");
            return false;
        }
    }
}
