using UnityEngine;
[System.Serializable]
public class IDType
{
    [UnityEngine.SerializeField]
    private int _id;
    public IDType(int id)
    {
        _id = id;
    }

    public int ID { get { return _id; } private set { _id = value; } }

    public override int GetHashCode()
    {
        int hashCode = (_id * 17) * 131 * 113 * 19;
        return hashCode;
    }

    public override bool Equals(object obj)
    {
        Debug.Log(obj.GetHashCode() + "   " + GetHashCode());
        if(obj.GetHashCode() == GetHashCode())
            return true;
        return false;
    }
}

