using UnityEngine;

public enum GameTags 
{
    //�̷������� ��ũ��Ʈ�� ���� enum�� ����ϸ� ��Ÿ�� ���ϼ� �ִ�
    Player,
    Enemy,
    Item,
    Bullet,

}
public class Tool 
{
    public static int rankCount = 10;
    public static string rankKey = "rankKey";
    public static bool isStating = false;
    public static string GetTag(GameTags _value) 
    {
        return _value.ToString();
    }
}

public class cUserDate 
{
    public string Name;
    public int Score;


}