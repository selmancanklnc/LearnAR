using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//1.需要添加一个按钮,拉开关闭
//2.需要刷新文本内容
public class Introduction_control : MonoBehaviour
{
    //是否处于真正显示状态
    public bool is_showing = false;

    //是否处于正在隐藏状态
    public bool is_hiding = false;

    [Header("文字描述背景的rect transform")]
    public RectTransform introduction_bg_rect_transform;

    [Header("文字描述的text")]
    public Text text_introduction;

    [Header("控制面板显示的按钮")]
    public GameObject game_obj_btn_show;

    //屏幕宽度
    private float screen_width;

    // Start is called before the first frame update
    void Start()
    {
        this.screen_width = Screen.width;        //print(w)

        this.screen_width = 1080;

        this.text_introduction.GetComponent<RectTransform>().sizeDelta = new Vector2(this.screen_width - 48, 0);

        this.introduction_bg_rect_transform.localPosition = new Vector3(-this.screen_width / 2, 0, 0);
    }

    void OnEnable()
    {
        this.reset_ready_show();
    }

    // Update is called once per frame
    private float interval_time = 0.025f;//间隔时间,单位秒
    private float time_stamp = 0;    //时间戳
    void Update()
    {
        //运动出现
        #region 
        if (this.is_showing)
        {
            this.time_stamp += Time.deltaTime;

            if (this.time_stamp > this.interval_time)
            {
                this.time_stamp = 0;

                //执行想要执行的东西
                float x = this.introduction_bg_rect_transform.localPosition.x;
                float y = this.introduction_bg_rect_transform.localPosition.y;
                float z = this.introduction_bg_rect_transform.localPosition.z;

                if (x >= this.screen_width / 2)
                {
                    this.is_showing = false;
                    this.introduction_bg_rect_transform.localPosition = new Vector3(this.screen_width / 2, y, z);
                    //this.game_obj_btn_show.SetActive(false);
                    return;
                }
                else
                {
                    this.introduction_bg_rect_transform.localPosition = new Vector3(x + 80, y, z);
                }
            }
        }
        #endregion

        //运动隐藏
        #region 
        else if (this.is_hiding)
        {
            this.time_stamp += Time.deltaTime;

            if (this.time_stamp > this.interval_time)
            {
                this.time_stamp = 0;

                //执行想要执行的东西
                float x = this.introduction_bg_rect_transform.localPosition.x;
                float y = this.introduction_bg_rect_transform.localPosition.y;
                float z = this.introduction_bg_rect_transform.localPosition.z;

                if (x <= -this.screen_width / 2)
                {
                    this.is_hiding = false;
                    this.introduction_bg_rect_transform.localPosition = new Vector3(-this.screen_width / 2, y, z);

                    //显示 或者隐藏 显示面板的按钮
                    if (Config.ar_statu == AR_statu.recognizing)
                    {
                        this.game_obj_btn_show.SetActive(false);
                    }
                    else
                    {
                        this.game_obj_btn_show.SetActive(true);
                    }
                    return;
                }
                else
                {
                    this.introduction_bg_rect_transform.localPosition = new Vector3(x - 80, y, z);
                }
            }
        }
        #endregion
    }

    //刷新文本内容数据
    public void refresh_text_content(int num)
    {
        string str = "";

        switch (num)
        {
            case 0:
                str = "The Jumeirah Sailing Hotel (Arabic: ??? ?????), also known as the Sailing Hotel, the Arabian Tower or the Bure porcelain hotel, is a luxury hotel in Dubai, United Arab Emirates, at 321 meters high, standing on an artificial island in the Persian Gulf 280 meters from the shore of the beach, connected by only a curved road. Managed by Jumeirah Luxury Hotels Group.  The hotel was founded in 1994 and opened to the public in December 1999. It looks like a dhow triangle sailboat (an Arab ship) and is purpose-built in a location that doesn't let the shadows of the building cover the beach. At the top of the hotel features a tarmac with a cantilevered beam structure protruding from the edge of the building.";
                break;
            case 1:
                str = "The Eiffel Tower (La Tour Eiffel, also commonly known as the Tower of Paris) is an iron-clad tower located in the 7th arrondissement of Paris, France, in the Place de seina de seine, a world-famous building and one of the symbols of French culture, one of the city landmarks of Paris, the tallest building in Paris. The official address is Rue Anatole-France 5.  The Eiffel Tower was built in 1889 and was first named Three Hundred Meters Tower and was later named after its designer, GustavE Eiffel. The Tower is a technical masterpiece in the history of world architecture and a paid-for monument in the world, with an estimated 6.98 million visitors in 2011, making it the second most visited cultural attraction in France. In 1991, the Eiffel Tower was listed as a World Heritage Site along the Seine in Paris.  The Eiffel Tower has occupied the position of the world's tallest man-made building for 40 years at a height of 312 meters. Its observation deck at 279.11 m is the highest observation deck available to the public in the European Union, second only to Moscow's Ostankino TV Tower in Europe. The total height of the tower has been increased several times by installing the antenna. These antennas have been used in many scientific experiments and are now mainly used to transmit radio and television signals.";
                break;
            case 2:
                str = "Located in Sydney, Australia, the Sydney Opera House is one of the most distinctive buildings of the 20th century and a world-famous performing arts centre and an iconic city of Sydney. The theatre was designed by Danish designer Jon Usson and construction work began in 1959 and the Grand Theatre was officially inaugurated in 1973. On 28 June 2007, the building was declared a World Heritage Site by UNESCO, and is one of the few 20th century-old buildings listed as a World Heritage Site from its completion to its inclusion in the list only 34 years.  Located at The convenient Bennelong Point in Sydney Harbour, the Sydney Opera House's unique sail shape, coupled with the sydney Harbour Bridge as a backdrop, is a hitch. Thousands of tourists come every day to see the building.  The Sydney Opera House consists mainly of two main halls, a number of small theatres, performance halls and other ancillary facilities. Both halls are located in a relatively large sail structure, while the small performance hall is located in the base at the bottom. The largest main hall is the concert hall, which can accommodate up to 2,679 people. The original design was to build the largest hall into an opera house, and the design was later changed, and even the completed opera stage was bulldozed and rebuilt. The concert hall contains a huge organ, made by Ronald Sharp from 1969 to 1979. It is said to be the world's largest mechanical wooden pipe organ, consisting of 10,500 ducts.  The smaller one in the main hall is the Opera House. Since the larger main hall was originally designed as an opera house, the small hall was considered not to be suitable for large opera performances, the stage was relatively small and the space given to the band was not easy for large bands to play.  Other ancillary facilities include a theatre, a cinema and a photography studio.";
                break;
            case 3:
                str = "The National Stadium is located in Chaoyang District Olympic Park in Beijing, People's Republic of China, is the main venue of the 2008 Beijing Olympic Games, due to its unique shape and commonly known as the Bird's Nest. The stadium has 100,000 seats during the Games and will host the opening and closing ceremonies of the Games, as well as athletics and soccer. The number of Olympic back-seats will be reduced to 80,000. In addition to the 2008 summer Olympics, the venue will be re-used at the 2022 Winter Olympics and the 2023 Asian Cup.  The National Stadium is 330 meters long, 220 meters wide, 69.2 meters high and covers an area of 250,000 square meters. The stadium, built with up to 121,000 tons of steel, will cost up to 2.3 billion yuan. The museum opened in December 2003 and officially opened in March 2004. However, due to the huge construction costs, the suspension of work in August 2004 had to be made. After a series of discussions and changes, the planned stadium's cover was removed, which experts say will not only make the stadium safer, but also cut some of the costs.The final stadium resumed construction in early 2005 and was completed on 28 June 2008.";
                break;
            case 4:
                str = "The Statue of Liberty(Liberty(English: Liberty Ing Ing Enlighten the World, French: La Libert ? It is a gift from the French people to the American people.The statue's figure is a woman in a robe, representing the reigning god of Roman mythology, holding the torch in her right hand, and writing the signing date of the American Declaration of Independence in Roman numerals on her left hand book: JULY IV MDCCLXXVI (July 4, 1776) with a broken chain at her feet. The statue is a symbol of freedom and America and a welcome sign for immigrants.  In 1865, The French law professor and politician Edouard Rene de LaVolaye proposed that France and the American people should work together to produce American independence memorabilia, which might have been conceived to commemorate the victory of the Northern Army and the end of slavery. It was inspired by LaVolaye that the statue was designed, but because the political situation in France was in trouble, the construction of the statue did not begin until the early 1870s. In 1875, LaVolaye proposed that France fund the statue, and that the United States provide the site and manufacture the base. Bartaldi made his head and held the torch before the sculpture was designed, and the parts were also on display at the international fair.";
                break;
            case 5:
                str = "The Leaning Tower of Pisa (Italian: Torre pendente di Pinsa or Torre di Pisa) is a tower located in the northern part of the pisa city complex in Pisa, Tuscany, Italy, and its adjacent cathedrals, baptists and cemeteries have a great influence on Italian architectural art from the 11th to the 14th centuries, and are therefore recognized as a World Heritage Site by the United Nations Educational, Scientific and Cultural Organization.  The Leaning Tower of Pisa, a freestanding clock tower of Pisa City Cathedral, behind the Cathedral of Pisa, is one of the three major buildings of Miracle Square, built in 1173 and designed to be built vertically, but tilted shortly after the start of the project due to uneven foundations and soft soil layers, completed in 1372 and tilted to the southeast.";
                break;
            case 6:
                str = "The Taj Mahal is a white marble mausoleum in Agra, Uttar Pradesh, India, and one of India's most famous monuments. It is the mausoleum of Shahjah Khan, the fifth emperor of the Mughal dynasty, in memory of his second wife, the late Queen Jiman Banu. Although the white marble dome mausoleum is the most familiar part of the Taj Mahal, the entire Taj Mahal is a complex complex of buildings with multiple buildings, including gates, courtyards and mosques, covering an area of 17 hectares.  Widely regarded as the building of Indian Muslim culture, the Taj Mahal is considered the most exquisite example of Mughal architecture by combining Indian and Persian architecture.  Construction of the Taj Mahal began around 1632 and was completed around 1653, using thousands of artisans and costing a lot. The design and construction of the Taj Mahal was entrusted to a team of architects, headed by ShahJahan, including architects such as Mir Abd-ul Karim, Makramat Khan and Ustad Ahmed Rahei. Among them, The Persian architect Laheli is generally considered to be the chief designer.  In 1983, UNESCO listed the Taj Mahal as a World Heritage Site, calling it the treasure of Indian Muslim art and one of the world's most impressive masterpieces in the World Heritage Site.";
                break;
            case 7:
                str = "The Egyptian pyramids are said to be the tombs of the ancient Egyptian pharaohs, but archaeologists have never found the pharaoh's mummies in the pyramids. Pyramids were popular in the ancient kingdom of Egypt. The base of the mausoleum is square, and on four sides are four equal triangles (i.e., square cones), with a silhouette similar to the gold character of a Chinese character, so Chinese called the pyramid. Pyramids are one of the seven wonders of the ancient world.  The Egyptian pyramids, one of the largest buildings to date, are one of the most influential and enduring symbols of ancient Egyptian civilization, mostofly built during the ancient and Central Egyptian kingdoms of Egypt.  Most research sources show that there are 81 to 112 pyramids in Egypt, while most scholars agree with the higher number. In 1842, Karl Richard Lepsius made a list of Egyptian pyramids, which he counted at 67, but later in his archaeological career had more pyramids that he had identified and discovered.";
                break;
            case 8:
                str = "Tower Bridge, a tower-tower iron bridge across the Thames in London, London, is also known for its history of building and overlooking the river, near the Tower of London.  London Tower Bridge is sometimes confused with London Bridge, which is another different bridge, just about a kilometre upstream of London Tower Bridge and is now one of London's most famous tourist attractions.";
                break;
            case 9:
                str = "The Pyramids of Castillo (Spanish: El Castillo, Spanish pronunciation: el kas' ti?o, meaning castles)) is located in the heart of the Mayan ruins of the Jugadon Peninsula, Chichen Itza, built between the 11th and 13th centuries, as a temple dedicated to the god of feathered snakes. All four staircases of the pyramids are 91th, but if the temple at the top of the pyramid is counted as a first order, the Castillo pyramids have a total of 365 orders, each of which represents every day of the Habs (Haab', one of the Mayan calendars).  At sunrise and sunset of the spring and autumn equinoxes, the corners of the pyramid cast a snake - like shadow on the north ladder and moved north of the pyramid as the sun moved. The pyramids of Castillo are 24 metres high, the temple tops six metres high and the base is about 55.3 meters long.The pyramids have nine layers, the same as Maya's view of the universe.";
                break;

            default:
                break;
        }


        this.text_introduction.text = str;
    }

    //重置到准备显示状态
    public void reset_ready_show()
    {
        this.is_hiding = true;
    }

    //事件
    #region 
    public void on_show_btn_event()
    {
        //播放按钮声音
        Audio_control.instance.play_btn_sound();

        //隐藏控制面板显示的按钮
        this.game_obj_btn_show.SetActive(false);

        //刷新面板问内容
        this.refresh_text_content(Config.building_index);

        //延时设置文本面板的位置,不延时 要报错
        StartCoroutine(this.set_panel_position());
    }


    //延时设置文本面板的位置
    public IEnumerator set_panel_position()
    {
        yield return new WaitForSeconds(0.05f);

        //print(">>>>>>" + this.introduction_bg_rect_transform.sizeDelta.y);

        //设置位置
        this.introduction_bg_rect_transform.localPosition = new Vector3(-this.screen_width / 2,  
            -this.introduction_bg_rect_transform.sizeDelta.y/2+50, 0);

        //显示面板
        this.is_showing = true;
    }


    public void on_test()
    {
        this.is_hiding = true;
    }
    #endregion
}
