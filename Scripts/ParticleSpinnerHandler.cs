using System.Threading.Tasks;
using UnityEngine;

public class ParticleSpinnerHandler
{   
    const float AMPLITUDE = 2;
    public async static Task StartSpinning(float radius,int numberOfLaps,float speed,
    Transform transformToAlter,Vector3 startPosition,float offset)
    {

        float startX = startPosition.x ;
        float startY = startPosition.y + offset;
        float startZ = startPosition.z ;
        

        int delay = (int) (100/ speed);
        for (float i = 0f; i <Mathf.PI * 2 * numberOfLaps; i+= 0.2f)
        {
            float x = Mathf.Cos(i) * radius;
            float z = Mathf.Sin(i) * radius;
            float y = Mathf.Sin(i * 0.2f) * AMPLITUDE;
            
            transformToAlter.position = new Vector3(startX +x,startY+ y,startZ +z);
            await Task.Delay(delay);
        }
        transformToAlter.position = new Vector3(startX,startY,startZ);
        
    }
}
