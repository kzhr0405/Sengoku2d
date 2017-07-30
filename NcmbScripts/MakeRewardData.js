module.exports = function (req, res) {
    var NCMB = require('ncmb');
    var ncmb = new NCMB("03c0d5d72cf57c3400ef2eae963af57f4190913accaed8b95aa8e039d203eeda", "6bb15d275c9d96732b5b08517ef9f1565a185756f16d0594e339d5714048c3dc");

    if(req.query.endDate == null){
        res.send("please input endDate (example: endDate=20170529)");
    }

    var endDate = Number(req.query.endDate);
    var pvpTmp = ncmb.DataStore("pvpTmp");

    LookFor();

    // rewardFlagがfalseのものを探し、trueに変えてGetRankを呼ぶ
    // fasleが0になったら終わり
    function LookFor(){
        pvpTmp.equalTo("endDate", endDate)
        .notEqualTo("totalWinNo", 0)
        .notEqualTo("rewardFlag", true)
        .limit(1)
        .count()
        .fetchAll()
        .then(function (results) {
            if (results.count >= 1) {
                results[0].set("rewardFlag", true);
                return results[0].update();
            }
            else res.json({ "message": "Finish!" });
        })
        .then(function (user) {
            GetRank(user);
        })
        .catch(function (err) {
            res.status(500)
                .json({ error: 500 });
        });
    }

    // 順位を算出してSetRewardを呼ぶ
    function GetRank(user) {
        pvpTmp.equalTo("endDate", endDate)
        .notEqualTo("atkNo", 0)
        .greaterThan("totalPt", user.get("totalPt"))
        .count()
        .fetchAll()
        .then(function (ptResults) {
            pvpTmp.equalTo("endDate", endDate)
            .notEqualTo("atkNo", 0)
            .equalTo("totalPt", user.get("totalPt"))
            .greaterThan("totalWinNo", user.get("totalWinNo"))
            .count()
            .fetchAll()
            .then(function (winNoResults) {
                SetReward(ptResults.count + winNoResults.count + 1, user.get("userId"));
            })
            .catch(function (err) {
                res.status(500)
                    .json({ error: 500 });
            });
        })
        .catch(function (err) {
            res.status(500)
                .json({ error: 500 });
        });
    }

    // 順位から報酬を決め、データストアに報酬データを格納する
    // その後LookForを呼ぶ
    function SetReward(rank, userId) {
        var title = "合戦場報酬" + rank + "位";
        var busyo = null;
        var busyoNum = null;
        var kaho = null;
        var kahoNum = null;
        var syokaijyo = null;
        var syokaijyoNum = null;
        var shiro = null;
        var money = null;

        if (rank == 1) {
            busyo = "S"
            busyoNum = 1;
            kaho = "S";
            kahoNum = 3;
            syokaijyo = "S";
            syokaijyoNum = 3;
            shiro = 1;
            money = null;
        } else if (rank == 2) {
            busyo = "S"
            busyoNum = 1;
            kaho = "S";
            kahoNum = 2;
            syokaijyo = "S";
            syokaijyoNum = 2;
            shiro = null;
            money = 150000;
        } else if (rank == 3) {
            busyo = "S"
            busyoNum = 1;
            kaho = "S";
            kahoNum = 1;
            syokaijyo = "S";
            syokaijyoNum = 1;
            shiro = null;
            money = 100000;
        } else if (4 <= rank && rank <= 10) {
            busyo = "A"
            busyoNum = 1;
            kaho = "S";
            kahoNum = 1;
            syokaijyo = "A";
            syokaijyoNum = 3;
            shiro = null;
            money = 80000;
        } else if (11 <= rank && rank <= 20) {
            busyo = "A"
            busyoNum = 1;
            kaho = "A";
            kahoNum = 3;
            syokaijyo = "A";
            syokaijyoNum = 2;
            shiro = null;
            money = 70000;
        } else if (21 <= rank && rank <= 50) {
            busyo = "A"
            busyoNum = 1;
            kaho = "A";
            kahoNum = 2;
            syokaijyo = "A";
            syokaijyoNum = 1;
            shiro = null;
            money = 60000;
        } else if (51 <= rank && rank <= 100) {
            busyo = "A"
            busyoNum = 1;
            kaho = "A";
            kahoNum = 1;
            syokaijyo = "B";
            syokaijyoNum = 3;
            shiro = null;
            money = 50000;
        } else if (101 <= rank && rank <= 200) {
            busyo = "B"
            busyoNum = 1;
            kaho = "B";
            kahoNum = 3;
            syokaijyo = "B";
            syokaijyoNum = 2;
            shiro = null;
            money = 40000;
        } else if (201 <= rank && rank <= 500) {
            busyo = "B"
            busyoNum = 1;
            kaho = "B";
            kahoNum = 2;
            syokaijyo = "B";
            syokaijyoNum = 1;
            shiro = null;
            money = 30000;
        } else if (501 <= rank && rank <= 1000) {
            busyo = "B"
            busyoNum = 1;
            kaho = "B";
            kahoNum = 1;
            syokaijyo = null;
            syokaijyoNum = null;
            shiro = null;
            money = 20000;
        } else if (1001 <= rank) {
            busyo = "C"
            busyoNum = 1;
            kaho = "C";
            kahoNum = 1;
            syokaijyo = null;
            syokaijyoNum = null;
            shiro = null;
            money = 10000;
        } else {
            res.json({ error: "rank error" });
        }

        // 報酬データの作成
        var Reward = ncmb.DataStore("reward");
        if (busyo != null) {
            var reward = new Reward({ title: title, grp: "busyo", rank: busyo, qty: busyoNum, userId: userId });
            reward.save()
                .then(function () {
                    LookFor();
                })
                .catch(function (err) {
                    res.status(500)
                        .json({ error: 500 });
                });
        }
        if (kaho != null) {
            var reward = new Reward({ title: title, grp: "kaho", rank: kaho, qty: kahoNum, userId: userId });
            reward.save()
                .catch(function (err) {
                    res.status(500)
                        .json({ error: 500 });
                });
        }
        if (syokaijyo != null) {
            var reward = new Reward({ title: title, grp: "syokaijyo", rank: syokaijyo, qty: syokaijyoNum, userId: userId });
            reward.save()
                .catch(function (err) {
                    res.status(500)
                        .json({ error: 500 });
                });
        }
        if (money != null) {
            var reward = new Reward({ title: title, grp: "money", rank: "C", qty: money, userId: userId });
            reward.save()
                .catch(function (err) {
                    res.status(500)
                        .json({ error: 500 });
                });
        }
        if (shiro != null) {
            var reward = new Reward({ title: title, grp: "shiro", rank: "C", qty: shiro, userId: userId });
            reward.save()
                .catch(function (err) {
                    res.status(500)
                        .json({ error: 500 });
                });
        }
    }
}