using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Camera FPCamera;
    [SerializeField] float shootRange = 100f;
    [SerializeField] float damage = 10f;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject hitEffect;

    // Update is called once per frame
    void Update()
    {
      if (Input.GetButtonDown("Fire1")){
          Shoot();
      } else if (Input.GetButtonUp("Fire1")){
          muzzleFlash.Stop();
      }
    }

    private void Shoot(){
        ProcessRaycast();
        PlayMuzzleFlash();
    }

    private void PlayMuzzleFlash(){
        muzzleFlash.Play();
    }

    private void ProcessRaycast(){
      RaycastHit hit;
      if(Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, shootRange)){
          CreateHitImpact(hit);
          EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
          if(target == null) return;

          target.TakeDamage(damage);
      }else{
          return;
      }
    }

    private void CreateHitImpact(RaycastHit hit){
        GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impact, 0.1f);
    }

}
