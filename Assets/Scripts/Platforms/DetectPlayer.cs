// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class DetectPlayer : MonoBehaviour
// {
//     private void OnTriggerEnter(Collider other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             TwoDCharacterMovement playerController = other.GetComponent<TwoDCharacterMovement>();
//             if (playerController != null)
//             {
//                 playerController.isUnderWall = true;
//             }
//         }
//     }
//
//     private void OnTriggerExit(Collider other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             TwoDCharacterMovement playerController = other.GetComponent<TwoDCharacterMovement>();
//             if (playerController != null)
//             {
//                 playerController.isUnderWall = false;
//             }
//         }
//     }
// }