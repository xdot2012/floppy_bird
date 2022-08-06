using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Level : MonoBehaviour
{
    private const float CAMERA_ORTHO_SIZE = 50f;
    private const float BLOCK_WIDTH = 10f;
    private const float HEAD_HEIGHT = 21f;
    private const float BLOCK_MOVE_SPEED = 30f;
    private const float BLOCK_DESTROY_X_POSITION = -150f;
    private const float BLOCK_SPAWN_X_POSITION = 150f;
    private const float MIN_GAP = 0.8f;

    // DEFINE TYPE OF BLOCKS IN THE LEVEL
    private const int BLOCK_TYPE = 3;

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
        SetStage();
    }


    private void Start()
    {
        GameAssets.getInstance().playStart();
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
            CreateGapBlock(BLOCK_SPAWN_X_POSITION);

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
                blockSpawnTimerMax = 4f;
                break;
            case Difficulty.Medium:
                gapSize = 40f;
                blockSpawnTimerMax = 3.5f;
                break;
            case Difficulty.Hard:
                gapSize = 33f;
                blockSpawnTimerMax = 3f;
                break;
            case Difficulty.Impossible:
                gapSize = 24f;
                blockSpawnTimerMax = 2f;
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
    private void CreateGapBlock(float xPosition) 
    {
        float randomize = Random.Range(0f, 1f);
        float scale = Random.Range(1f, 5f);
        // Only Bottom
        if (randomize <= 0.35f)
        {
            CreateBlock(scale, xPosition, true);
        }
        // Only Top
        else if (randomize <= 0.7f)
        {
            CreateBlock(scale, xPosition, false);
        }
        // Top and Bottom
        else
        {
            float maxTop = (2f - scale);
            // Check if Minimun Space
            if (maxTop < MIN_GAP) maxTop = 0.5f;
            float scaleTop = Random.Range(0.5f, maxTop);
            CreateBlock(scale, xPosition, true);
            CreateBlock(scaleTop, xPosition, false);
        }
        blocksSpawned++;
        SetDifficulty(GetDifficulty());
    }

    private void CreateBlock(float randScale, float xPosition, bool createBotton)
    {

        //Set up Block Head
        //Choose block type
        Transform blockHead = ChooseBlockHead(BLOCK_TYPE);

        //Set the block Up or Down
        float blockHeadYPosition;
        if (createBotton)
        {
            blockHeadYPosition = -CAMERA_ORTHO_SIZE + HEAD_HEIGHT * .35f * randScale;
           
        }
        else
        {
            blockHeadYPosition = +CAMERA_ORTHO_SIZE - HEAD_HEIGHT * .35f * randScale;
        }


        //Set Scale
        //blockHead.localScale = new Vector3(1, randScale, 1);

        //Set Position
        //xPosition = xPosition + Random.Range(-25f, 25f);
        blockHead.position = new Vector3(xPosition, blockHeadYPosition);

        if (!createBotton)
        {
            blockHead.Rotate(0, 180, 180);
        }
        blockHead.Rotate(0, 0, Random.Range(50f, -50f));
        //Set up Block Body

        //Set the block Up or Down
        //Choose block type
        Transform blockBody = ChooseBlockBody(BLOCK_TYPE);
        
        float blockBodyYPosition;
        if (createBotton)
        {
            blockBodyYPosition = -CAMERA_ORTHO_SIZE;

            //Set Scale
            blockBody.localScale = new Vector3(1, randScale, 1);

        } else
        {
            blockBodyYPosition = +CAMERA_ORTHO_SIZE;

            //Set Scale
            blockBody.localScale = new Vector3(1, -randScale, 1);
        }
        blockBody.position = new Vector3(xPosition, blockBodyYPosition);



        SpriteRenderer blockHeadRenderer = blockHead.GetComponent<SpriteRenderer>();
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

    //Choose block head type
    private Transform ChooseBlockHead(int level)
    {
        return Instantiate(GameAssets.getInstance().pfPipa);

        if (level == 1)
        {
            return Instantiate(GameAssets.getInstance().pfBlockHead);
        }
        else if (level == 2)
        {
            return Instantiate(GameAssets.getInstance().pfBlockHead2);
        }
        else if (level == 3)
        {
            return Instantiate(GameAssets.getInstance().pfBlockHeadWax);
        }
        else
        {
            return Instantiate(GameAssets.getInstance().pfBlockHead);
        }
    }

    //Choose block head type
    private Transform ChooseBlockBody(int level)
    {
        return Instantiate(GameAssets.getInstance().pfCerol);

        if (level == 1)
        {
            return Instantiate(GameAssets.getInstance().pfBlockBody);
        }
        else if (level == 2)
        {
            return Instantiate(GameAssets.getInstance().pfBlockBody2);
        }
        else if (level == 3)
        {
            return Instantiate(GameAssets.getInstance().pfBlockBodyWax);
        }
        else
        {
            return Instantiate(GameAssets.getInstance().pfBlockBody);
        }
    }

    private void SetStage()
    {
        return;
        // Set Floor and Roof
        if (BLOCK_TYPE == 1)
        {
            Transform Background = Instantiate(GameAssets.getInstance().pfBackground);
            Transform Ground = Instantiate(GameAssets.getInstance().pfGround);
            Transform Roof = Instantiate(GameAssets.getInstance().pfGround);

            Background.position = new Vector3(0, 0, 0);
            Ground.position = new Vector3(0, 50, 0);
            Ground.Rotate(0, 180, 180);
            Roof.position = new Vector3(0, -50, 0);
        }
        else if (BLOCK_TYPE == 2)
        {
            Transform Background = Instantiate(GameAssets.getInstance().pfBackground2);
            Transform Ground = Instantiate(GameAssets.getInstance().pfGround2);
            Transform Roof = Instantiate(GameAssets.getInstance().pfGround2);

            Background.position = new Vector3(0, 0, 0);
            Ground.position = new Vector3(0, 50, 0);
            Ground.Rotate(0, 180, 180);
            Roof.position = new Vector3(0, -50, 0);
        }
        else if (BLOCK_TYPE == 3)
        {
            Transform Background = Instantiate(GameAssets.getInstance().pfBackgroundWax);
            Transform Ground = Instantiate(GameAssets.getInstance().pfGroundWax);
            Transform Roof = Instantiate(GameAssets.getInstance().pfGroundWax);

            Background.position = new Vector3(0, 0, 0);
            Ground.position = new Vector3(0, 50, 0);
            Ground.Rotate(0, 180, 180);
            Roof.position = new Vector3(0, -50, 0);

        }
        else
        {
            Transform Background = Instantiate(GameAssets.getInstance().pfBackground);
            Transform Ground = Instantiate(GameAssets.getInstance().pfGround);
            Transform Roof = Instantiate(GameAssets.getInstance().pfGround);

            Background.position = new Vector3(0, 0, 0);
            Ground.position = new Vector3(0, 50, 0);
            Ground.Rotate(0, 180, 180);
            Roof.position = new Vector3(0, -50, 0);
        }



    }
}

