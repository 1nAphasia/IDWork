using UnityEngine;
using Unity.Cinemachine;
using StarterAssets;
using UnityEngine.ProBuilder;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;
using UnityEditor.PackageManager;

public class ThirdShooterController : MonoBehaviour
{
    public GameObject CrossHair;
    [SerializeField] private CinemachineCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;
    [SerializeField] private Transform pfBulletProjectile;
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] private Animator animator;
    private int CurrentWeapon = 0;
    private List<RuntimeWeaponInstance> weapons;
    private FireMode currentFireMode = FireMode.SemiAuto;
    private float shootTimer = 0f;
    private bool isReloading = false;
    private float reloadTimer = 0f;
    private const float reloadDuration = 3f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        animator = GetComponent<Animator>();
        CurrentWeapon = GameDataManager.I.StatsService.playerStats.CurrentWeapon;
        weapons = GameDataManager.I.StatsService.playerStats.Weapons;
        currentFireMode = weapons[CurrentWeapon].AvailableFireMode[0];

        starterAssetsInputs.OnRPressed += TryReload;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 ScreenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(ScreenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            mouseWorldPosition = raycastHit.point;
        }
        if (starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            CrossHair.SetActive(true);
            thirdPersonController.SetRotateOnMove(false);
            thirdPersonController.SetSensitivity(aimSensitivity);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            aimDirection = Quaternion.AngleAxis(40, Vector3.up) * aimDirection;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else
        {
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
            CrossHair.SetActive(false);

        }
        // if (starterAssetsInputs.shoot)
        // {
        //     Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
        //     Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        //     starterAssetsInputs.shoot = false;
        // }
        if (Keyboard.current.vKey.wasPressedThisFrame && weapons[CurrentWeapon].AvailableFireMode.Count > 1)
        {
            int idx = weapons[CurrentWeapon].AvailableFireMode.IndexOf(currentFireMode);
            idx = (idx + 1) % weapons[CurrentWeapon].AvailableFireMode.Count;
            currentFireMode = weapons[CurrentWeapon].AvailableFireMode[idx];
            // 可在此处调用UI刷新方法
        }

        // 射击逻辑
        shootTimer += Time.deltaTime;
        bool canShoot = shootTimer >= 60f / weapons[CurrentWeapon].RateOfFire;

        if (currentFireMode == FireMode.SemiAuto)
        {
            if (starterAssetsInputs.shoot && canShoot && starterAssetsInputs.aim && weapons[CurrentWeapon].CurrentAmmo > 0 && !isReloading)
            {
                ShootRay(mouseWorldPosition);
                shootTimer = 0f;
                starterAssetsInputs.shoot = false;
                weapons[CurrentWeapon].CurrentAmmo -= 1;
                UIManager.Instance.UpdateCurrentAmmo(weapons[CurrentWeapon].CurrentAmmo);
            }
        }
        else if (currentFireMode == FireMode.FullAuto)
        {
            if (starterAssetsInputs.shoot && canShoot && starterAssetsInputs.aim && weapons[CurrentWeapon].CurrentAmmo > 0 && !isReloading)
            {
                ShootRay(mouseWorldPosition);
                shootTimer = 0f;
                weapons[CurrentWeapon].CurrentAmmo -= 1;
                UIManager.Instance.UpdateCurrentAmmo(weapons[CurrentWeapon].CurrentAmmo);
            }
        }

        // 换弹计时
        if (isReloading)
        {
            ReloadTick();
        }


    }
    private void ShootRay(Vector3 targetPos)
    {
        Vector3 shootDir = (targetPos - spawnBulletPosition.position).normalized;
        Ray ray = new Ray(spawnBulletPosition.position, shootDir);
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderLayerMask))
        {
            // 处理命中逻辑，如扣血、特效等
            var target = hit.collider.GetComponent<BulletTarget>();
            var EnemyStats = hit.collider.GetComponent<EnemyMono>();
            if (target != null)
            {
                EnemyStats.DamageDelt(100);
                UIManager.Instance.ShowHitNumber(100);
                // 处理目标受伤
            }
            // 可实例化特效
            // Instantiate(hitVFX, hit.point, Quaternion.identity);
        }
        // 可播放射击动画、音效等
    }

    public void ChangeWeaponStatus(int i)
    {
        CurrentWeapon = i;
        UIManager.Instance.UpdateAmmoPanel(weapons[CurrentWeapon].CurrentAmmo, weapons[CurrentWeapon].AmmoLeft, weapons[CurrentWeapon].AvailableFireMode[0]);
    }

    public void TryReload()
    {
        if (!isReloading && weapons[CurrentWeapon].CurrentAmmo < weapons[CurrentWeapon].MaxAmmo && weapons[CurrentWeapon].AmmoLeft > 0)
        {
            isReloading = true;
            reloadTimer = 0f;
            UIManager.Instance.ReloadStatusBar.gameObject.SetActive(true);
            NotificationManager.I.AddNotification("Reloading...");
        }
    }
    public void ReloadTick()
    {
        reloadTimer += Time.deltaTime;
        UIManager.Instance.UpdateReloadStatusBar(reloadTimer / reloadDuration);
        if (reloadTimer >= reloadDuration)
        {
            int needAmmo = weapons[CurrentWeapon].MaxAmmo - weapons[CurrentWeapon].CurrentAmmo;
            int takeAmmo = Mathf.Min(needAmmo, weapons[CurrentWeapon].AmmoLeft);
            weapons[CurrentWeapon].CurrentAmmo += takeAmmo;
            weapons[CurrentWeapon].AmmoLeft -= takeAmmo;
            isReloading = false;
            UIManager.Instance.UpdateAmmoPanel(weapons[CurrentWeapon].CurrentAmmo, weapons[CurrentWeapon].AmmoLeft, currentFireMode);
            NotificationManager.I.AddNotification("Reload Complete.");
            UIManager.Instance.ReloadStatusBar.gameObject.SetActive(false);

        }
        return; // 换弹期间不能射击
    }
}
