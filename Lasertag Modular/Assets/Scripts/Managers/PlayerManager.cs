using Managers;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private int _maxLifes = 5;
    private int _currentLifes = 5;

    public void SetLife(int life)
    {
        _currentLifes = life;
    }

    public int Hit()
    {
        _currentLifes--;

        if (_currentLifes < 0)
        {
            //CALL SERVER DEATH
        }

        return _currentLifes;
    }

    public int Heal()
    {
        _currentLifes++;

        _currentLifes = Mathf.Clamp(_currentLifes, 0, _maxLifes);

        return _currentLifes;
    }
}
