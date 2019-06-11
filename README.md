# Block Breaker

## Projenin Özeti

O eski Block Breaker oyununu çoğu kişi bilir. Eski bilgisayarlarımızda da sıkça oynadığımız oyunlar arasındaydı ki daha sonrasında klonları da sürekli yapılmaya devam edildi. 2 yıl önce bu oyunun bir klonunu geliştirmiştim ve şimdi ise remastered edilmiş haliyle tekrardan oyunu yazdım ve sizlerin de faydalanması için GitHub'a ekliyorum. 

## Proje İçin Gereksinimler:

1-) Temel düzeyde C# bilmeniz gerekir. Hatta temel düzeyde herhangi bir OOP konseptli programlama dili bilmeniz bile yeterli aslında. Ancak proje C# üzerinde yazıldığı için C# bilmeniz daha avantajlı olur. 

2-) Temel düzeyde Unity bilgisi gereklidir. Çünkü canvasın ne olduğu, prefab ve inspector gibi kavramların ne anlama geldiğini anlatmayacağım. Daha çok projenin algoritmasını ve nasıl çalıştığını anlatacağım. Yani bu projeyi alırken size tavsiyem Unity arayüzünü ve biraz da detayını bilmenizdir. 

3-) Unity'nin 2018.2.0f sürümünü kullanarak bu projeyi yazdım. Size yine bu sürümü indirmenizi şiddetle tavsiye ediyorum. Şu an hala Unity 2018.3 sürümüne geçiş yapmadığım için o sürümde çalışıp çalışmadığını bilmiyorum. Eski sürümlerde de denemedim, 2018.1 ve 2017.3-2017.4 sürümlerinde tahmin ediyorum ki hatasız açılabilir. 

## Proje Hakkında Bilgi

Öncelikle oyunu bilmiyorsanız kısaca oyundan bahsedeyim. Elinizde bir palet ve bu palete sabitlenmiş bir top var. Bu topla kutuları vurmaya çalışıyorsunuz. Aslında çok da karmaşık bir oynanışa sahip değil. Yukarıda saydığım gereksinimlere sahipseniz rahat bir şekilde projenin çalışma mantığını kavrayabilirsiniz. 

Öncelikle oyunda 6 adet scriptimiz var. Bu scriptlere gelmeden önce de size projenin arayüzünden kısaca bahsedeyim. Oyunun arayüzü genel olarak şu şekilde: 

<a href="https://ibb.co/gTz1wBK"><img src="https://i.ibb.co/fp4RtPw/Block-Breaker.jpg" alt="Block-Breaker" border="0"></a>

Arayüzde bir platform, onun üzerinde yer alan bir top ve bloklar var. Sahnenin alt kısmında bir Lose Collider mevcut. Yani top, bu nesneyle collide olursa trigger aktif oluyor ve bu da game over demek. Direkt olarak SceneManager adlı Unity'nin kendi sahne yönetimi kütüphanesindeki SceneManager sınıfından aldığımız LoadScene metodunu kullanarak Game Over sahnesini çağırıyoruz. Tek satırlık oldukça basit olan script şu şekilde:

```C# 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene("Game Over");
    }
```

Geliştirme ekranında ise şu şekilde duvarlar ekledim ve böylece topun bağımsızlığını ilan edip ekranın dışına gitmesini engellemiş oldum: 

<a href="https://imgbb.com/"><img src="https://i.ibb.co/KK5pzC6/Block-Breaker.jpg" alt="Block-Breaker" border="0"></a>

Sahnemizde olanlar ise kısaca resimdeki gibi. Arkaplanımız, platformumuz, topumuz, bloklarımız var. Level nesnemiz, levelin o anki geçilebilirlik durumunu kontrol edecek. GameState nesnemiz de oyunumuzun statüsünü belirleyecek ve canvasımız bu nesnede bulunacak. Skorları da yansıtmak için bu nesneyi kullanacağız:

<a href="https://imgbb.com/"><img src="https://i.ibb.co/jgLL5x4/Block-Breaker.jpg" alt="Block-Breaker" border="0"></a>

Diğer scriptimiz Paddle yani topu taşıdığımız platformun scripti de şu şekilde: 

```C#
    [SerializeField] private float screenWidthInUnits = 16;
    [SerializeField] private float minX = 1f;
    [SerializeField] private float maxX = 15f;
    
   	// Update is called once per frame
  	void Update ()
	  {
	     float mousePosInUnit = Input.mousePosition.x / Screen.width * screenWidthInUnits;
	     Vector2 _paddlePos = new Vector2(mousePosInUnit, transform.position.y);
	     _paddlePos.x = Mathf.Clamp(mousePosInUnit, minX, maxX);
	     transform.position = _paddlePos;
    }
```
Yukarıdaki scriptimizde öncelikle ekran genişliğinin referansını aldık ve 16 birim olarak değerini verdik. Siz, kendi kullandığınız arkaplanın boyutuna göre bunu değiştirebilirsiniz. 1440x1080 boyutunda olduğu için birim başına 90 pixel olarak arkaplan görüntüsünü ayarladım ve bu doğrultuda 16 birim ekran genişliği belirledim. X ekseninde gidilebilecek maksimum ve minimum değerleri de 1 ve 15 olarak girdim. 

Update fonksiyonunda ise mouse pozisyonunu birim olarak alabilmek için x eksenindeki pozisyonunu ekran genişliğine böldük ve birim olarak ekran genişliği değeri ile çarptık. Yani bu bize farenin pozisyonunu birim olarak hesaplayabilme imkanı verdi. Daha sonra bir Vector 2 tanımladık ve bu vektörde x değerini fare pozisyonu olarak alırken y değerini ise olduğu gibi bıraktık. Birim cinsinden fare pozisyonunun belirlediğimiz maksimum ve minimum x değerlerinin dışına çıkmamasını sağlamak için de 3.satırda Mathf.Clamp fonksiyonunu kullandık. Bu fonksiyon, belirli değerler aralığında bir değişkenin değer almasına imkan verir ama bu değerlerin dışına çıkmamasını sağlar. Detaylı bilgi için Unity dökümanlarını okuyabilirsiniz: https://docs.unity3d.com/ScriptReference/Mathf.Clamp.html. paddlePos isimli bir değişken tanımladık ve bu değişkenin x ekseninin Mathf.Clamp fonksiyonunda tanımladığımız aralıkta değer almasını sağladık.
Son olarak, platformun pozisyon değerini de paddlePos değerine eşitledik. Böylece bu kod bize platformumuzu sağa sola hareket ettirme imkanı verdi. 

Bir diğer önemli script ise top scripti: 

```C#
    [SerializeField] private Paddle paddle;  //platforma referans aldık. 
    [SerializeField] private bool hasStarted = false; //oyunun başlayıp başlamadığını belirleyen bool değişkeni 
    [SerializeField] private Vector2 paddleToBallVector; //topu platforma sabitlerken kullanacağımız vektör
    [SerializeField] private float xPush = 2f; //topun x ekseninde aldığı ivmenin değeri
    [SerializeField] private float yPush = 15f; // topun y ekseninde aldığı ivmenin değeri
    [SerializeField] private AudioClip[] ballSounds; // topun herhangi bir nesneyle temas ettiğinde çıkardığı sesler için oluşturduğum array
    [SerializeField] private float randomFactor = 0.2f; // topun hareketinde tek düze sekmeleri engellemek adına ivmeye eklenen random değer aralığının üst sınırı
    private AudioSource myAudioSource;
    private Rigidbody2D myRigidbody2D;
    
    void Start ()
	  {
       paddleToBallVector = transform.position - paddle.transform.position;
	     myAudioSource = GetComponent<AudioSource>();
	     myRigidbody2D = GetComponent<Rigidbody2D>();
	  }
  
    private void LockBallToPaddle()
    {
        Vector2 paddlePos = new Vector2(paddle.transform.position.x, paddle.transform.position.y);
        transform.position = paddlePos + paddleToBallVector;
    }
```
Oyunda önceliğimiz, topu platforma sabitlemek. Peki bunu nasıl yapabiliriz? Vector2 cinsinden bir paddleToBallVector değeri tanımlamıştık. Bu vektöre öncelikle bir değer belirlemeliyiz. Bunu da, topun o anki pozisyonu ile platformun pozisyonu arasındaki farkı bularak yapabiliriz. Böylece aradaki mesafeyi bu vektör içinde tutmuş olacak. 

LockBallToPaddle() fonksiyonunda ise ayrıyeten bir paddlePos vektörü tanımladık ve bu vektöre platformun x ve y değerlerini attık. Son olarak da, topun instantiate edileceği pozisyonu ise platformun pozisyonu ve daha önceden aldığımız paddleToBallVector değerlerini ekleyerek bulduğumuz yeni pozisyon olarak belirledik. Böylece Unity, topu sürekli olarak o pozisyonda tutacak ve biz de arzuladığımız sabit top pozisyonunu elde etmiş olacağız.

Topun hareketi için ayrıca PhysicsMaterial2D adlı bir özellikten faydalandık. Bu özellik, kodlama kısmında bize kolaylık sağladı ve topun elastiklik değerini değiştirmemize olanak verdi. Bounciness adlı çarpışmalarda elastiklik veren değeri 1 yaptım ve friction değerine ellemedim.   

```C#
  void Update ()
	{
	    if (!hasStarted)
	    {
	        LockBallToPaddle();
	        LaunchBallOnClick();
        }
  }

  private void LaunchBallOnClick()
  {
      if (Input.GetMouseButtonDown(0))
      {
          myRigidbody2D.velocity = new Vector2(xPush, yPush);
          hasStarted = true;
      }    
  }
```
Şimdi de topu hareket ettirmemiz lazım. Bunun için de Rigidbody2D sınıfını kullanacağız ve bu sınıftan velocity adlı hız vektörünü çağıracağız. Hız vektörüne ise, x ve y eksenlerinde belirlediğimiz itiş değerlerini ekleyeceğiz ve oyunun başlangıcı için hazırladığımız hasStarted değerini true yapacağız ki Start fonksiyonunda diğer metodlarımızı aktif edip kullanabilelim. 

```C#
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 velocityTweak = new Vector2(UnityEngine.Random.Range(0f, randomFactor), UnityEngine.Random.Range(0f, randomFactor));
        if (hasStarted)
        {
            AudioClip clip = ballSounds[UnityEngine.Random.Range(0, ballSounds.Length)];
            myAudioSource.PlayOneShot(clip);
            myRigidbody2D.velocity += velocityTweak;
        }
    }
```
Topu hareket ettirdik, top bloklara çarptı. Çarptığı zaman çarpış sesinin çalması lazım. Bunun için ise normal bir AudioClip tanımlayıp tek bir ses atabilirsiniz ama ben array olarak tanıttım ve birden fazla sesin random şekilde çalmasını istedim. Bu nedenle yukarıdaki gibi bir random range attım ve dizinin uzunluğuna göre random sesler çıksın istedim. PlayOneShot metodunu kullanmanızı şiddetle tavsiye ederim. Bu metod, top hızlıca birden fazla bloğa çarparsa sürekli yeni ses çalmayacaktır ve öncekinin bitmesini bekleyecektir. Ses karışıklığı yaratmaması adına bunu kullanmanızı öneririm. Yoksa normal metodla da müziği çalabilirsiniz, size kalmış. 

Oyunu test ederken fark ettiğim bir detay da sürekli bir döngü içinde topun bazen yavaşça ilerlemesi. O duvardan bu duvara sürekli gidiyor ve uzun bir süre döngüden çıkmıyor. Bunun için de çözümü bir random değer atayarak belirlemeye başladım. Yeni bir vektör daha tanıttım ve bu vektörde x ve y değerlerine belirlediğim random değer ile 0 arasında bir değer girdim. Bu vektörü de rigidbody den aldığım hız referansı ile topladım ve böylece her collisionda bu değer ile top biraz daha ivme kazanacak ve döngüye girmeyecek ya da döngüye girdiğinde daha kısa bir sürede çıkmayı başaracak. 

Bir de level scriptimiz var. Bu script kısaca sahnedeki blokların sayısını alıyor ve bu blokların daha sonrasında sıfıra gelmesi durumunda build ederken belirlediğiniz sıralamaya göre sonraki levelin gelmesini sağlıyor. 

```C#
    [SerializeField] private int breakableBlocks;
    private SceneLoader sceneLoader;

    void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    public void CountBreakableBlocks()
    {
        breakableBlocks++;
    }

    public void BlocksAreDestroyed()
    {
        breakableBlocks--;
        if (breakableBlocks == 0)
        {
            sceneLoader.LoadNextScene();
        }
    }
```
Scriptte adı geçen SceneLoader scripti ise şu şekilde: 

```C#
public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene(0);
        FindObjectOfType<GameSession>().ResetGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
```    
Yine bu scriptte GameSession'dan referans aldık. O scriptte şu şekilde: 
```C#
[SerializeField]
    [Range(0.1f,1f)] private float gameSpeed = 1f; //oyunun hızını belirliyorsunuz. Inspector'da range cinsinden tanıttım böylece çubuk üzerinde gameplay test yaparak uygun değeri seçebilirsiniz kendiniz.
    [SerializeField] private TextMeshProUGUI scoreText; //Skor için canvasda gözükecek text.
    [SerializeField] private int scoreAddition = 50; //Skorun artacağı aralık
    [SerializeField] private int currentScore = 0; //Sahip olunan skor, oyunun başı olduğu için sıfıra sabitledim. 

    void Awake()
    {
        int gameStatusCount = FindObjectsOfType<GameSession>().Length;
        if (gameStatusCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
	// Update is called once per frame
	void Update ()
	{ 
      //oyunun hızını ayarlıyorsunuz. Time.timeScale değeri, uygulamanızın oynatılma hızını belirler. 1 yaparsanız gerçek zamanla eş gider. 0.5 yaparsanız 2 kat daha yavaş gider, isterseniz 1'den fazla da yapabilirsiniz.
	    Time.timeScale = gameSpeed;
	}

    public void AdditionToTheScore()
    {
    //skoru güncelliyorsunuz. ToString metodu ile de canvasa yansıtıyorsunuz.
        currentScore += scoreAddition;
        scoreText.text = currentScore.ToString();
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }
```

Diğer bir scriptimiz ise Block scripti: 

```C#
    [SerializeField] private AudioClip breakSound; //bloğun kırıldığında çıkan ses
    [SerializeField] private GameObject blockSparklesVFX; //blok kırıldığına çıkacak partikül efekti
    [SerializeField] private int timesHit = 0; // çarpma sayısı
    [SerializeField] private Sprite[] hitSprites; //bloklar kırıldığında yerine gelecek spriteların array olarak referansı
    
    private Level level; // leveller için aldığımız referans
    private int spriteIndex; // sprite arrayında ilerlemek için kullanacağımız index değeri
    
    void Start()
    {
        level = FindObjectOfType<Level>();
        if (tag == "Breakable")
        {
            level.CountBreakableBlocks();
        }
        FindObjectOfType<GameSession>();
    }  
```
Oyun başladığında öncelikle Level sınıfından referans alıyoruz ve bloklara attığımız Breakable tagi ile eşleşen bloklar olursa Level sınıfında tanımladığımız CountBreakableBlocks() sınıfını çağırıyoruz. Method içerisinde de sahnedeki kırılabilen blokların sayısı hesaplanıyor.

```C#
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tag == "Breakable")
        {
            HandleHits();
        }
        FindObjectOfType<GameSession>().AdditionToTheScore();
    }
    
    private void HandleHits()
    {
        timesHit++;
        int maxHits = hitSprites.Length + 1;
        if (timesHit == maxHits)
        {
            DestroyBlocks();
        }
        else
        {
            ShowNextHitSprite();
        }
    }
    private void ShowNextHitSprite()
    {
        spriteIndex = timesHit - 1;
        if (hitSprites[spriteIndex] != null)
        {
           GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
        }
        
    }
    
    private void TriggerSparklesVFX()
    {
        GameObject sparkles = Instantiate(blockSparklesVFX, transform.position, transform.rotation);
        Destroy(sparkles, 1f);
    }
    
    private void DestroyBlocks()
    {
        AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position);
        Destroy(gameObject);
        TriggerSparklesVFX();
        level.BlocksAreDestroyed();
    }
```
Bloklara çarptığımızda eğer o blok kırılabilen bir blok ise direkt olarak HandleHits metoduna ilerliyoruz. Bu metodda ise çarpışmanın sayısı bir arttırılıyor ve maksimum çarpışam değeri de arrayimizin uzunluğunun bir fazlası olarak belirleniyor ki arrayde belirlediğimiz tüm kırılma spritelarına adım adım erişilebilsin. En sonunda çarpışma hitleri eğer ki maksimum hitlerle eşitlenirse DestroyBlocks metoduna gidiyoruz ve o metodda önce PlayClipAtPoint metodunu kullanarak kamera üzerinden(blok üzerinden sesi alırsanız kalitesi düzgün olmaz, tavsiye etmem pek) sesi oynatıyor ve objeyi yok ediyor. TriggerSparklesVFX() metodu da projeye eklediğim partikül efekti bloğun pozisyonunda Instantiate ediyor ve 1 saniye süreyle oynatıyor.

Eğer hitlerimiz o an maksimum hite ulaşmadıysa bloğun zarar gördüğünü belirten spritelarımızı prefab üzerinde değiştirmemiz lazım. ShowNextHitSprite() metodu tam olarak bunu yapıyor. spriteIndex değerimiz, bu süreçte arrayin içerisinde ilerleyecek ve vuruş sayısına göre dizide ilerleyip sıradaki resmi gösterecek. 

## Sonuç

Elimden geldiği kadar projemi sizlere anlatmaya çalıştım. Eğer anlamadığınız bir yer olursa mutlaka bana ulaşın ve elimden geldiği kadar sizlere yardımcı olayım. Proje açık kaynaktır ve assetleri kullanma konusunda herhangi bir sınırlamaya sahip değilsiniz. Kodları ve assetleri istediğiniz gibi kullanabilirsiniz. Projede planladığım çok şey vardı ama zaman sorunundan dolayı maalesef hepsini yetiştiremedim. Örnek olarak kırılamayan bloklar ekleyerek farklı bir challenge yapabilirsiniz. Farklı bloklar kırıldığında farklı şeyleri trigger edebilir, mesela oyunun hızını arttırıp düşürebilir. Eski Block Breaker versiyonunlarından birinde top birden fazla parçaya bölünebiliyordu, bunu da ekleyebilirsiniz mesela. Top boyutunu arttırabilir veya farklı bloklar collide olduğunda platformu küçültebilirsiniz. Çok fazla opsiyonunuz var. Oynanış testi yaparak kendi zevkinize göre oyunu dizayn edebilirsiniz. Desteklerinizi bekliyorum ve bol kodlu günler diliyorum :)

