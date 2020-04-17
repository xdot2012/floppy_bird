using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private const float CAMERA_ORTHO_SIZE = 50f;
    private const float BLOCK_WIDTH = 10f;
    private const float HEAD_HEIGHT = 21f;
    private const float BLOCK_MOVE_SPEED = 30f;
    private const float BLOCK_DESTROY_X_POSITION = -100f;
    private const float BLOCK_SPAWN_X_POSITION = 100f;

    private List<Block> blockList;
    private int blocksSpawned;
    private float blockSpawnTimer;
    private float blockSpawnTimerMax;
    private float gapSize;

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Impossible
    }

    private void Awake()
    {
        blockList = new List<Block>();
        blockSpawnTimerMax = 1f;
        SetDifficulty(Difficulty.Easy);
    }

    private void Start()
    {
       // CreateGapBlock(50f, 20f, 20f);
    }

    private void Update()
    {
        if (!Player.dead)
        {
            handleBlockMovement();
            handleBlockSpawning();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space)) { Restart(); }
        }
    }

    private void handleBlockSpawning()
    {
        blockSpawnTimer -= Time.deltaTime;
        if(blockSpawnTimer < 0)
        {
            //Time to Spawn another block
            blockSpawnTimer = blockSpawnTimerMax;

            float heightHeadLimit = 10f;
            float minHeight = gapSize * .5f + heightHeadLimit;
            float totalHeight = CAMERA_ORTHO_SIZE * 2f;
            float maxHeight = totalHeight - gapSize * .5f - heightHeadLimit;

            float height = Random.Range(minHeight, maxHeight);
            CreateGapBlock(height, gapSize, BLOCK_SPAWN_X_POSITION);

        }
    }

    private void handleBlockMovement()
    {
        for (int i=0; i < blockList.Count; i++)
        {
            Block block = blockList[i];
            block.Move();   
            if(block.getXPosition() < BLOCK_DESTROY_X_POSITION)
            {
                //Destroy Block
                block.destroySelf();
                blockList.Remove(block);
                i--;
            }
        }
    }

    //Dificulty
    private void SetDifficulty(Difficulty difficulty)
    {
        switch(difficulty)
        {
            case Difficulty.Easy:
                gapSize = 50f;
                blockSpawnTimerMax = 1.2f;
                break;
            case Difficulty.Medium:
                gapSize = 40f;
                blockSpawnTimerMax = 1.1f;
                break;
            case Difficulty.Hard:
                gapSize = 33f;
                blockSpawnTimerMax = 1f;
                break;
            case Difficulty.Impossible:
                gapSize = 24f;
                blockSpawnTimerMax = .7f;
                break;
        }
    }

    private Difficulty GetDifficulty()
    {
        if (blocksSpawned >= 30) return Difficulty.Impossible;
        if (blocksSpawned >= 20) return Difficulty.Hard;
        if (blocksSpawned >= 10) return Difficulty.Medium;
        return Difficulty.Easy;
    }
    
    
    //Set up two Blocks with a Gap in the middle
    private void CreateGapBlock(float gapY, float gapSize, float xPosition) 
    {
        CreateBlock(gapY - gapSize * .5f, xPosition, true);
        CreateBlock(.5f, xPosition, true);
        CreateBlock(CAMERA_ORTHO_SIZE * 2f - gapY - gapSize * .5f, xPosition, false);
        blocksSpawned++;
        SetDifficulty(GetDifficulty());
    }

    private void CreateBlock(float height, float xPosition, bool createBotton)
    {

        //Set up Block Head

        //Set the block Up or Down
        float blockHeadYPosition;
        if (createBotton)
        {
            blockHeadYPosition = -CAMERA_ORTHO_SIZE + HEAD_HEIGHT * .5f;
           
        }
        else
        {
            blockHeadYPosition = +CAMERA_ORTHO_SIZE - HEAD_HEIGHT * .5f;
        }
        Transform blockHead = Instantiate(GameAssets.getInstance().pfBlockHead);
        blockHead.position = new Vector3(xPosition, blockHeadYPosition);
        blockHead.localScale = new Vector3(1, 0.5f, 1);
        if (!createBotton)
        {
            blockHead.Rotate(0, 180, 180);
        }
        //Set up Block Body

        //Set the block Up or Down
        Transform blockBody = Instantiate(GameAssets.getInstance().pfBlockBody);
        float blockBodyYPosition;
        if (createBotton)
        {
            blockBodyYPosition = -CAMERA_ORTHO_SIZE;
        } else
        {
            blockBodyYPosition = +CAMERA_ORTHO_SIZE;
            blockBody.localScale = new Vector3(1, -1, 1);
        }
        blockBody.position = new Vector3(xPosition, blockBodyYPosition);


        SpriteRenderer blockHeadRenderer = blockHead.GetComponent<SpriteRenderer>();
        blockHeadRenderer.size = new Vector2(BLOCK_WIDTH, height);

        PolygonCollider2D blockHeadPolygonCollider = blockHead.GetComponent<PolygonCollider2D>();

        Block block = new Block(blockHead, blockBody);
        blockList.Add(block);
    }

    private void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }



    //Represents a Entire Block
    private class Block
    {

        private Transform blockHeadTransform;
        private Transform blockBodyTransform;

        public Block(Transform blockHeadTransform, Transform blockBodyTransform)
        {
            this.blockHeadTransform = blockHeadTransform;
            this.blockBodyTransform = blockBodyTransform;
        }

        public void Move()
        {
            blockHeadTransform.position += new Vector3(-1, 0, 0) * BLOCK_MOVE_SPEED * Time.deltaTime;
            blockBodyTransform.position += new Vector3(-1, 0, 0) * BLOCK_MOVE_SPEED * Time.deltaTime;

        }

        public float getXPosition() {
            return blockHeadTransform.position.x;
        }

        public void destroySelf()
        {
            Destroy(blockHeadTransform.gameObject);
            Destroy(blockBodyTransform.gameObject);
        }
    }
}
