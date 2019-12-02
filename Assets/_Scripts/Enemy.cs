using UnityEngine;
using UnityEngine.Events;
using Behavioral;
using System;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    [SerializeField] public int _health;
    [SerializeField] public int _maxHealth;

    public Image _healthBarBackground;
    public Image _healthBar;

    [SerializeField] int _attackDamage;
    [SerializeField] int _goldValue;
    [SerializeField] bool _golden;

    Castle _castle;
    Player _player;
    SplineFollower _splineFollower;
    WaveManager _waveManager;
    
    public UnityEvent onDeath;
    public UnityEvent onCastleHit;

    Rigidbody _rigidbody;
    Animator _animator;

    bool isKilledByTowerProjectile;

    private const float EPSILON = 0.0001f;
	
    void Awake() {
        _health = _maxHealth;
        _castle = GameObject.Find("Castle").GetComponent<Castle>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _splineFollower = GetComponent<SplineFollower>();
        _waveManager = GameObject.Find("WaveManager").GetComponent<WaveManager>();
        _rigidbody = GetComponentInChildren<Rigidbody>();
        _animator = GetComponent<Animator>();

        // increase health every 2 rounds
        int waveNumber = _waveManager.GetWaveNumber();
        if(waveNumber % 2 == 1)
            _health += (_waveManager.GetWaveNumber() - 1) * 3;
    }

    void LateUpdate() {
        CheckProgress();
        _animator.SetBool("isKilledByTower", isKilledByTowerProjectile);
    }


    public void TakeDamage(int value) {
        _health -= value;
        _healthBar.fillAmount = (float)_health / (float)_maxHealth;
        CheckHealth();
    }

    void CheckProgress() {
        if(Mathf.Abs(_splineFollower.GetPercentageOfSplineProgress() - 1.0f) < EPSILON) {
            _castle.TakeDamage(_attackDamage);
            onCastleHit?.Invoke();
            Destroy(this.gameObject);
        }
    }


    public float GetProgress() {
        return Mathf.Abs(_splineFollower.GetPercentageOfSplineProgress() - 1.0f);
    }

    void CheckHealth() {
        if(_health <= 0) {
            Destroy(_healthBarBackground);
            isKilledByTowerProjectile = true;
            _splineFollower.speedInUnitsPerSecond = 0.0f;
            _rigidbody.transform.position = new Vector3(_rigidbody.transform.position.x, -30.0f, _rigidbody.transform.position.z);
        }
    }

    void OnTriggerEnter(Collider other) {
        if ( other.gameObject.CompareTag("Castle") ) {
            _castle.TakeDamage(_attackDamage);
        }
    }

    public void Die() {
        if(_golden)
            _player.GainGold(_goldValue);
        onDeath?.Invoke();
        Destroy(gameObject);
    }
}
