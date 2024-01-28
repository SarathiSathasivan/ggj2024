using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class Shitpost : MicroGame {
    [SerializeField] private TMP_Text postText;
    [SerializeField] private TMP_Text replyText;
    [SerializeField] private Image keyboard;
    [SerializeField] private Image replyFrame;

    private string _reply;
    private int replyIndex = 0;

    private readonly string[] originalPosts = {
        "Last of us was mid",
        "I like turtles",
        "IATAH for wanting to leave my family?"
    };

    private string[] shitPosts = {
        "What the fuck did you just fucking say about me, you little bitch? I’ll have you know I graduated top of my " +
        "class in the Navy Seals, and I’ve been involved in numerous secret raids on Al-Quaeda, and I have over " +
        "300 confirmed kills.",
        "Did you ever hear the tragedy of Darth Plagueis The Wise? I thought not. It’s not a story the Jedi would " +
        "tell you. It’s a Sith legend. Darth Plagueis was a Dark Lord of the Sith, so powerful and so wise he could " +
        "use the Force to influence the midichlorians to create life…",
        "According to all known laws of aviation, there is no way that a bee should be able to fly. Its wings are " +
        "too small to get its fat little body off the ground. The bee, of course, flies anyways. Because bees don't " +
        "care what humans think is impossible."
    };

    private void Start() {
        keyboard.gameObject.SetActive(false);
        replyFrame.gameObject.SetActive(false);
        Random random = new();
        postText.text = originalPosts[random.Next(0, originalPosts.Length)];
        replyText.text = "";
        _reply = shitPosts[random.Next(0, shitPosts.Length)];
    }

    public void Reply() {
        keyboard.gameObject.SetActive(true);
        replyFrame.gameObject.SetActive(true);
    }

    public void Autocomplete() {
        replyText.text = string.Join(" ", _reply.Split(" ").Take(++replyIndex));
        if (replyIndex > 10) {
            GameManager.Instance.FinishMicroGame(true);
        }
    }
}
