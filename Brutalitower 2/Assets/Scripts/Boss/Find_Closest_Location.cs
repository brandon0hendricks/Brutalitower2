using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Find_Closest_Location : MonoBehaviour
{

    //Diffrent locations for location
    private Transform center_location;
    private Transform Left_location;
    private Transform right_location;


    private void Start()
    {
        Set_transform();

    }

    private void Update()
    {
        Debug.Log(FindClosestLocation());
    }
    private void Set_transform() //This will Set transforms from game manager 
    {

        Left_location = Game_Manager.instance.BossRoomLocations[0];
        center_location = Game_Manager.instance.BossRoomLocations[1];
        right_location = Game_Manager.instance.BossRoomLocations[2];


    }

    public string FindClosestLocation() //this finds the location the player is closest to
    {
        
        GameObject player = Game_Manager.instance.player; //Get player game object
        float rightLocDistance = Vector3.Distance(player.transform.position, right_location.position); //Get Distance from Player to right location
        float leftLocDistance = Vector3.Distance(player.transform.position, Left_location.position); //Get Distance from Player to left location
        float centerLocDistance = Vector3.Distance(player.transform.position, center_location.position); //Get Distance from Player to center location



        //return string of whichever distance is the closest

        if(rightLocDistance < centerLocDistance && rightLocDistance < leftLocDistance)
        {
            return ("Right");
        }
        else if(centerLocDistance <= rightLocDistance && centerLocDistance <= leftLocDistance)
        {
            return ("Center");
        }
        else if(leftLocDistance < centerLocDistance && leftLocDistance < rightLocDistance)
        {
            return ("left");
        }
        else
        {
            return null;
        }
    }
}
