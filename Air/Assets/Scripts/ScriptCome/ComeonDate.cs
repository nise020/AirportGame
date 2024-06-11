using UnityEngine;

public enum GameTags 
{
    //이런식으로 스크립트를 만들어서 enum을 사용하면 오타를 줄일수 있다
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