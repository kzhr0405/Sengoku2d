module.exports = function (req, res) {
    var NCMB = require('ncmb');
    var ncmb = new NCMB("03c0d5d72cf57c3400ef2eae963af57f4190913accaed8b95aa8e039d203eeda", "6bb15d275c9d96732b5b08517ef9f1565a185756f16d0594e339d5714048c3dc");

    var pvpTmp = ncmb.DataStore("pvpTmp");

    Loop();

    // rewardFlagが全てfalseになるまで変更し続ける再帰関数
    function Loop() {
        pvpTmp.notEqualTo("rewardFlag", false)
            .limit(1)
            .count()
            .fetchAll()
            .then(function (results) {
                if (results.count >= 1) {
                    results[0].set("rewardFlag", false);
                    return results[0].update();
                }
                else res.json({ "message": "Finish!" });
            })
            .then(function () {
                Loop();
            })
            .catch(function (err) {
                res.status(500)
                    .json({ error: 500 });
            })
    }
}