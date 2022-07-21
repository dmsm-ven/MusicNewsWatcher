DROP TABLE IF EXISTS Tracks;
DROP TABLE IF EXISTS Albums;
DROP TABLE IF EXISTS Artists;
DROP TABLE IF EXISTS MusicProviders;
DROP TABLE IF EXISTS Settings;

DROP TABLE IF EXISTS MusicProviders;
CREATE TABLE IF NOT EXISTS MusicProviders (
	MusicProviderId	int NOT NULL PRIMARY KEY AUTO_INCREMENT,
	Name	TEXT NOT NULL,
	Image	TEXT NULL,
	Uri	TEXT NULL
);
DROP TABLE IF EXISTS Settings;
CREATE TABLE IF NOT EXISTS Settings (
	Name	varchar(256) NOT NULL PRIMARY KEY UNIQUE,
	Value	TEXT NOT NULL
);

DROP TABLE IF EXISTS Artists;
CREATE TABLE IF NOT EXISTS Artists (
	ArtistId	INTEGER NOT NULL PRIMARY KEY AUTO_INCREMENT,
	MusicProviderId	INTEGER NOT NULL,
	Name	TEXT NOT NULL,
	Uri	TEXT NULL,
	Image	TEXT NULL,
	CONSTRAINT FK_Artists_MusicProviders_MusicProviderId FOREIGN KEY(MusicProviderId) REFERENCES MusicProviders(MusicProviderId)
);

DROP TABLE IF EXISTS Albums;
CREATE TABLE IF NOT EXISTS Albums (
	AlbumId	INTEGER NOT NULL PRIMARY KEY AUTO_INCREMENT,
	ArtistId	INTEGER NOT NULL,
	Title	TEXT NOT NULL,
	Created	TEXT NOT NULL,
	IsViewed	INTEGER NOT NULL,
	Image	TEXT,
	Uri	TEXT,
	CONSTRAINT FK_Albums_Artists_ArtistId FOREIGN KEY(ArtistId) REFERENCES Artists(ArtistId) ON DELETE CASCADE
);

DROP TABLE IF EXISTS Tracks;
CREATE TABLE IF NOT EXISTS Tracks (
	Id	INTEGER NOT NULL PRIMARY KEY AUTO_INCREMENT,
	AlbumId	INTEGER NOT NULL,
	Name	TEXT NOT NULL,
	DownloadUri	TEXT,
	CONSTRAINT FK_Tracks_Albums_AlbumId FOREIGN KEY(AlbumId) REFERENCES Albums(AlbumId) ON DELETE CASCADE
);

INSERT INTO MusicProviders VALUES (1,'Bandcamp','','https://bandcamp.com'),
 (2,'Musify','','https://musify.club');
INSERT INTO Settings VALUES ('DownloadThreadsNumber','2'),
 ('TelegramApiToken','5126269910:AAEpsrsmxtfcgZro96xbQGyHrVeXqstcLsM');
INSERT INTO Artists VALUES (1,1,'Alphaxone','https://alphaxone.bandcamp.com','https://thumbnailer.mixcloud.com/unsafe/300x300/extaudio/0/b/7/7/cdaa-5a92-4ea6-b246-0e30755d6f57'),
 (2,1,'Boris Brejcha','https://borisbrejcha.bandcamp.com','https://i.scdn.co/image/ab6761610000e5eb33c9b5dcc05194199aa68410'),
 (3,1,'Cryo Chamber','https://cryochamber.bandcamp.com','https://i.ytimg.com/an/000z5zd6mrc/10750502086316475689_mq.jpg?v=622f96fc'),
 (4,1,'E-Mantra','https://e-mantra.bandcamp.com','https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQJuXclDTTvTy8nESQsvP-5ziZAe4Sknva2vA&usqp=CAU'),
 (5,1,'Dahlia''s Tear','https://dahliastear1.bandcamp.com','https://i.discogs.com/Jtz3R_G-vCabrysDAKpShJ6ecvndM3aFhEHNtJwp534/rs:fit/g:sm/q:90/h:529/w:600/czM6Ly9kaXNjb2dz/LWRhdGFiYXNlLWlt/YWdlcy9SLTE0NDUx/OTA2LTE1NzQ3OTA4/NjYtMTgwNy5qcGVn.jpeg'),
 (6,1,'Neurotech','https://neurotech.bandcamp.com','https://i.discogs.com/sZB2K45YTJzxZ5T0y1vpb1czHq_kp6T3weZ5JjyOFVw/rs:fit/g:sm/q:90/h:500/w:500/czM6Ly9kaXNjb2dz/LWRhdGFiYXNlLWlt/YWdlcy9SLTY2MjUw/ODQtMTQyMzMzOTQ0/OC05NTM4LmpwZWc.jpeg'),
 (7,1,'Global Sect','https://globalsect.bandcamp.com','https://i1.sndcdn.com/avatars-000039378453-lncn9t-t500x500.jpg'),
 (8,1,'Ionomusic','https://ionomusic.bandcamp.com/','https://is1-ssl.mzstatic.com/image/thumb/Music113/v4/e5/07/9f/e5079fd8-db71-84f1-78cb-650ff6ab686b/192641065521.png/400x400cc.jpg'),
 (9,1,'SunTripRecords','https://suntriprecords.bandcamp.com','https://i1.sndcdn.com/avatars-rSZ0zbyQXa97xMny-NOkmhA-t500x500.jpg'),
 (10,1,'Ovnimoon Records','https://ovnimoonrecords.bandcamp.com','https://geo-media.beatport.com/image_size/500x500/6e5353ae-9c07-4fc7-a35b-0348b775df83.jpg'),
 (11,1,'ElectricUniverse','https://electric-universe.bandcamp.com/','https://is5-ssl.mzstatic.com/image/thumb/Music42/v4/2d/74/c8/2d74c8aa-f76e-1157-dd1c-de1c2842e414/cover.jpg/400x400cc.jpg'),
 (12,1,'Juno Reactor','https://junoreactor.bandcamp.com','https://lastfm.freetls.fastly.net/i/u/300x300/faa0133b014b4e87a77ea845d5ed4d3a.jpg'),
 (13,1,'Talamasca','https://talamascamusic.bandcamp.com','https://avatars.yandex.net/get-music-content/2357076/ec230eb3.a.11001455-1/m1000x1000'),
 (14,1,'Ginger Snap5','https://gingersnap5.bandcamp.com','https://lastfm.freetls.fastly.net/i/u/300x300/37eb944a9b524a12af2697d7bafa2687.jpg'),
 (15,2,'Alphaxone','https://musify.club/artist/alphaxone-168598','https://39s-a.musify.club/img/70/11021413/29376128.jpg'),
 (16,2,'Dynazty','https://musify.club/artist/dynazty-38226','https://sun9-4.userapi.com/impf/c629315/v629315154/2ea11/F-mHdREmiNI.jpg?size=594x604&quality=96&sign=72fd97e23c286978b4dc155359bb9e74&type=album'),
 (17,2,'Neurotech','https://musify.club/artist/neurotech-68844','https://i.scdn.co/image/ab67616d0000b27379a8ea351f87b5c6cfe6927f'),
 (18,2,'Powerwolf','https://musify.club/artist/powerwolf-23054','https://i4.stat01.com/2/624/106233895/afacdb/powerwolf-jpg.jpg'),
 (19,2,'Mental Discipline','https://musify.club/artist/mental-discipline-83037','https://avatars.yandex.net/get-music-content/4076749/29729272.a.15581872-1/m1000x1000'),
 (20,2,'Psy-H Project','https://musify.club/artist/psy-h-project-176661','https://lastfm.freetls.fastly.net/i/u/ar0/586e4e7fa034d87a66c5f25efb695771.jpg'),
 (21,2,'Sphäre Sechs','https://musify.club/artist/sphare-sechs-168601','https://i.scdn.co/image/ab67616d00001e027d86333a5be2cdc07f23da37'),
 (22,2,'Council of Nine','https://musify.club/artist/council-of-nine-374471','https://is4-ssl.mzstatic.com/image/thumb/Music5/v4/67/5f/c1/675fc164-d740-ca54-9934-276394adf14b/889211353811.jpg/600x600bf-60.jpg'),
 (23,2,'Dahlia''s Tear','https://musify.club/artist/dahlias-tear-23193','http://sun9-85.userapi.com/s/v1/ig1/o48vjSI4s_PAM8MbJTR1L7r6fxUPIboKVMl62RRtVf1X8jxtRR4LAV2nCUhNlA4fFGb8SHst.jpg?size=604x533&quality=96&type=album'),
 (26,2,'Melodysheep','https://musify.club/artist/melodysheep-735333','https://lastfm.freetls.fastly.net/i/u/500x500/9b49d0e087ef953e023fa849ec792a45.jpg'),
 (27,2,'Atrium Carceri','https://musify.club/artist/atrium-carceri-13805','https://www.thisisdarkness.com/wp-content/uploads/2018/11/cover.jpg'),
 (28,2,'God Dody Disconnect','https://musify.club/artist/god-body-disconnect-418139','https://avatars.yandex.net/get-music-content/176019/b2959221.a.7310431-1/m1000x1000?webp=false'),
 (29,2,'Elepho','https://musify.club/artist/elepho-281903','https://avatars.yandex.net/get-music-content/34131/be646fe4.a.2822624-1/m1000x1000?webp=false'),
 (30,2,'Juno Reactor','https://musify.club/artist/juno-reactor-2005','https://avatars.yandex.net/get-music-content/2442093/08729501.a.60258-3/m1000x1000?webp=false'),
 (31,2,'Infected Mushroom','https://musify.club/artist/infected-mushroom-991','https://avatars.yandex.net/get-music-content/2397565/ac74fc68.a.10759170-1/m1000x1000?webp=false'),
 (32,2,'Median Project','https://musify.club/artist/median-project-538986','https://avatars.yandex.net/get-music-content/163479/661e6644.a.7712986-1/m1000x1000?webp=false'),
 (33,2,'Talamasca','https://musify.club/artist/talamasca-9632','https://is3-ssl.mzstatic.com/image/thumb/Music/v4/1d/5a/bd/1d5abd96-ebc7-192d-95fa-aacb645242e1/cover.jpg/1200x1200bf-60.jpg'),
 (34,2,'Boris Brejcha','https://musify.club/artist/boris-brejcha-15493','https://41s.musify.club/img/68/12541689/32612430.jpg'),
 (35,2,'E-Mantra','https://musify.club/artist/e-mantra-78075','https://avatars.yandex.net/get-music-content/33216/13a673ac.a.1261356-1/m1000x1000'),
 (36,2,'Insomnium','https://musify.club/artist/insomnium-16578','https://i1.stat01.com/2/6735/167345996/075a3e/insomnium-shadows-of-the-dying-sun.jpg'),
 (37,2,'Sybreed','https://musify.club/artist/sybreed-4542','https://41s-a.musify.club/img/70/1144191/40246304.jpg');
INSERT INTO Albums VALUES (8602,15,'Ghost Machine','2021-12-05 00:00:00',0,'https://41s.musify.club/img/68/24856556/62762573.jpg','https://musify.club/release/alphaxone-ghost-machine-2021-1559835'),
 (8603,15,'Endless Horizon','2021-12-06 00:00:00',0,'https://39s.musify.club/img/68/24874374/62773380.jpg','https://musify.club/release/alphaxone-endless-horizon-2021-1560478'),
 (8604,15,'Sunset Chillout Area PT2','2021-06-10 00:00:00',0,'https://38s.musify.club/img/68/24048953/61034396.jpg','https://musify.club/release/sunset-chillout-area-pt2-2021-1505626'),
 (8605,15,'The Deep Sleep Music','2021-05-18 00:00:00',0,'https://39s.musify.club/img/68/23879708/60677839.jpg','https://musify.club/release/the-deep-sleep-music-2021-1494696'),
 (8606,15,'Back to Beyond','2021-05-08 00:00:00',0,'https://38s.musify.club/img/68/23825417/60533828.jpg','https://musify.club/release/back-to-beyond-2021-1489619'),
 (8607,15,'Dream Chambers','2020-08-25 00:00:00',0,'https://38s.musify.club/img/69/21883547/55817677.jpg','https://musify.club/release/dream-chambers-2020-1356311'),
 (8608,15,'Dystopian Gate','2020-08-13 00:00:00',0,'https://38s.musify.club/img/69/21804539/55531818.jpg','https://musify.club/release/alphaxone-dystopian-gate-2020-1341457'),
 (8609,15,'Velvet Sunset: Chillout Balearic. Vol.2','2020-10-20 00:00:00',0,'https://38s.musify.club/img/68/22360298/57630933.jpg','https://musify.club/release/velvet-sunset-chillout-balearic-vol-2-2020-1390574'),
 (8610,15,'Chill Out Recharge, Vol. 2','2020-12-18 00:00:00',0,'https://40s.musify.club/img/68/22796181/58375649.jpg','https://musify.club/release/chill-out-recharge-vol-2-2020-1416158'),
 (8611,15,'Shadows Of Forgotten Legends','2020-08-11 00:00:00',0,'https://38s.musify.club/img/69/21791152/55517750.jpg','https://musify.club/release/shadows-of-forgotten-legends-2020-1339072'),
 (8613,15,'Edge of Solitude','2018-06-23 00:00:00',0,'https://41s-a.musify.club/img/71/16529764/43911527.jpg','https://musify.club/release/alphaxone-edge-of-solitude-2018-1009945'),
 (8614,15,'Chill Out Dub Goa Trance 2018 Top 100 Hits DJ Mix','2020-09-11 00:00:00',0,'https://39s.musify.club/img/68/22028876/56563089.jpg','https://musify.club/release/chill-out-dub-goa-trance-2018-top-100-hits-dj-mix-2018-1374015'),
 (8615,15,'Aftermath','2018-11-09 00:00:00',0,'https://37s-a.musify.club/img/71/17524440/46150902.jpg','https://musify.club/release/aftermath-2018-1066532'),
 (8616,15,'Stardust','2017-02-18 00:00:00',0,'https://41s.musify.club/img/69/13092591/33779133.jpg','https://musify.club/release/stardust-2017-817697'),
 (8617,15,'Tomb Of Seers','2017-02-18 00:00:00',0,'https://41s.musify.club/img/69/13092618/33779164.jpg','https://musify.club/release/tomb-of-seers-2017-817695'),
 (8618,15,'Chill Out 2018 Top 40 Hits (CD1)','2020-09-13 00:00:00',0,'https://40s.musify.club/img/68/22047294/57318623.jpg','https://musify.club/release/chill-out-2018-top-40-hits-cd1-2017-1374653'),
 (8619,15,'Forsaken','2017-09-04 00:00:00',0,'https://41s-a.musify.club/img/71/14551607/37081152.jpg','https://musify.club/release/forsaken-2017-901136'),
 (8620,15,'Terra Relicta Presents: Vol. I Dark Ambient','2017-09-28 00:00:00',0,'https://41s-a.musify.club/img/71/14729222/37491546.jpg','https://musify.club/release/terra-relicta-presents-vol-i-dark-ambient-2016-911789'),
 (8621,15,'Echoes From Outer Silence','2016-03-06 00:00:00',0,'https://39s-a.musify.club/img/70/11021413/29376128.jpg','https://musify.club/release/alphaxone-echoes-from-outer-silence-2016-701553'),
 (8622,15,'Absence Of Motion','2015-08-30 00:00:00',0,'https://39s-a.musify.club/img/71/9985370/26103608.jpg','https://musify.club/release/alphaxone-absence-of-motion-2015-644628'),
 (8623,15,'Altered Dimensions','2015-03-27 00:00:00',0,'https://39s-a.musify.club/img/71/9171433/24439329.jpg','https://musify.club/release/alphaxone-altered-dimensions-2015-608278'),
 (8624,15,'Tomb Of Empires','2015-09-15 00:00:00',0,'https://39s.musify.club/img/68/10045927/26222997.jpg','https://musify.club/release/tomb-of-empires-2014-647398'),
 (8625,15,'Living In The Grayland','2014-03-20 00:00:00',0,'https://38s-a.musify.club/img/70/6889799/19797630.jpg','https://musify.club/release/alphaxone-living-in-the-grayland-2014-483391'),
 (8626,15,'Dark Complex','2013-11-15 00:00:00',0,'https://39s-a.musify.club/img/70/6002800/18184953.jpg','https://musify.club/release/alphaxone-dark-complex-2013-438297'),
 (8627,15,'Across The Grooves','2015-09-04 00:00:00',0,'https://39s.musify.club/img/68/10010251/26147923.jpg','https://musify.club/release/alphaxone-across-the-grooves-2013-645708'),
 (8628,15,'The Curtain Of Silence','2013-03-10 00:00:00',0,'https://39s.musify.club/img/69/4758267/15469656.jpg','https://musify.club/release/alphaxone-the-curtain-of-silence-2013-368288'),
 (8629,15,'Azimuth','2015-09-15 00:00:00',0,'https://39s.musify.club/img/68/10045894/26222962.jpg','https://musify.club/release/alphaxone-azimuth-2012-647396'),
 (8630,15,'Phase.O.N.E','2015-09-15 00:00:00',0,'https://39s.musify.club/img/68/10045553/26222910.jpg','https://musify.club/release/alphaxone-phase-o-n-e-2012-647394'),
 (8631,15,'Grounds','2015-09-15 00:00:00',0,'https://39s.musify.club/img/68/10045498/26222902.jpg','https://musify.club/release/alphaxone-grounds-2012-647393'),
 (8632,15,'Invisible','2012-09-20 00:00:00',0,'https://39s.musify.club/img/69/3962130/15487004.jpg','https://musify.club/release/alphaxone-invisible-2012-329511'),
 (8633,15,'Synthetic Vision','2012-09-26 00:00:00',0,'https://38s.musify.club/img/69/3990782/16912631.jpg','https://musify.club/release/alphaxone-synthetic-vision-2012-333389'),
 (8634,15,'Exploration','2015-09-15 00:00:00',0,'https://39s.musify.club/img/68/10045910/26222983.jpg','https://musify.club/release/alphaxone-exploration-2011-647397'),
 (8635,15,'Portal','2015-09-09 00:00:00',0,'https://39s.musify.club/img/69/10023045/26178063.jpg','https://musify.club/release/alphaxone-portal-2011-646395'),
 (8636,15,'Endurance','2015-09-09 00:00:00',0,'https://39s.musify.club/img/69/10023038/26178056.jpg','https://musify.club/release/alphaxone-endurance-2011-646394'),
 (8637,15,'Nucleus MS-106','2015-09-09 00:00:00',0,'https://39s.musify.club/img/69/10023005/26178040.jpg','https://musify.club/release/alphaxone-nucleus-ms-106-2010-646393'),
 (8638,15,'Elliptical Mono-X','2015-09-09 00:00:00',0,'https://39s.musify.club/img/69/10022948/26178029.jpg','https://musify.club/release/alphaxone-elliptical-mono-x-2010-646390'),
 (8639,31,'Another Dimension','2022-03-26 00:00:00',0,'https://38s.musify.club/img/68/25475105/63928720.jpg','https://musify.club/release/another-dimension-2022-1603343'),
 (8640,31,'The Synthpop Disco','2022-03-28 00:00:00',0,'https://38s.musify.club/img/68/25477668/63951573.jpg','https://musify.club/release/the-synthpop-disco-2022-1603830'),
 (8641,31,'Guarana Cupana: Psy Sound Energetic Mix','2021-04-08 00:00:00',0,'https://38s.musify.club/img/68/23600759/60054466.jpg','https://musify.club/release/guarana-cupana-psy-sound-energetic-mix-2021-1473545'),
 (8642,31,'Gravity Grave: Psy Trance Set','2021-04-06 00:00:00',0,'https://38s-a.musify.club/img/71/23580524/60016336.jpg','https://musify.club/release/gravity-grave-psy-trance-set-2021-1472792'),
 (8643,31,'New Music Releases Week 21 Pt.5','2021-06-03 00:00:00',0,'https://39s.musify.club/img/68/24024082/60923078.jpg','https://musify.club/release/new-music-releases-week-21-pt-5-2021-1501261'),
 (8644,31,'Loud Remixes, vol. 2','2021-01-31 00:00:00',0,'https://38s-a.musify.club/img/71/23074897/58957940.jpg','https://musify.club/release/loud-remixes-vol-2-2021-1435170'),
 (8645,31,'(Original Mix) (Extended Mix) CD38','2021-04-21 00:00:00',0,'https://39s.musify.club/img/68/23705140/60296079.jpg','https://musify.club/release/original-mix-extended-mix-cd38-2021-1482533'),
 (8646,31,'Infected Mushroom Works Collection Part 2','2020-03-17 00:00:00',0,'https://38s.musify.club/img/69/20644030/52912261.jpg','https://musify.club/release/infected-mushroom-works-collection-part-2-2020-1263439'),
 (8647,31,'Infected Mushroom Works Collection Part 1','2020-03-17 00:00:00',0,'https://38s.musify.club/img/69/20644027/52912172.jpg','https://musify.club/release/infected-mushroom-works-collection-part-1-2020-1263438'),
 (8648,31,'More Than Just A Name','2020-03-24 00:00:00',0,'https://39s.musify.club/img/68/20711962/53019864.jpg','https://musify.club/release/infected-mushroom-more-than-just-a-name-2020-1266216'),
 (8649,31,'Cartoon People: Future House Vol. 2','2020-11-02 00:00:00',0,'https://38s.musify.club/img/68/22453461/57729300.jpg','https://musify.club/release/cartoon-people-future-house-vol-2-2020-1395682'),
 (8650,31,'(Original Mix) (Extended Mix) CD25','2020-05-19 00:00:00',0,'https://40s-a.musify.club/img/71/21135941/53930746.jpg','https://musify.club/release/original-mix-extended-mix-cd25-2020-1291577'),
 (8651,31,'The Story Of Hadag Nahash','2020-03-08 00:00:00',0,'https://39s.musify.club/img/69/20566386/52769530.jpg','https://musify.club/release/the-story-of-hadag-nahash-2020-1259579'),
 (8652,31,'The Works Of Psyfactor','2020-05-28 00:00:00',0,'https://39s.musify.club/img/69/21202575/54075502.jpg','https://musify.club/release/the-works-of-psyfactor-2020-1294943'),
 (8653,31,'Ani Mevushal','2020-01-22 00:00:00',0,'https://40s-a.musify.club/img/71/20314021/52150557.jpg','https://musify.club/release/ani-mevushal-2020-1241173'),
 (8654,31,'The G.M.S. Experiment','2020-06-29 00:00:00',0,'https://40s-a.musify.club/img/71/21479783/54593534.jpg','https://musify.club/release/the-g-m-s-experiment-2020-1308820'),
 (8655,31,'Louder Than Loud','2020-04-25 00:00:00',0,'https://40s-a.musify.club/img/71/20945883/53521911.jpg','https://musify.club/release/louder-than-loud-2020-1280604'),
 (8656,31,'Monstercat - Best Of 2020','2020-12-24 00:00:00',0,'https://38s.musify.club/img/68/22836910/58449105.jpg','https://musify.club/release/monstercat-best-of-2020-2020-1418627'),
 (8657,31,'The Sound Of Fractal','2020-06-02 00:00:00',0,'https://37s-a.musify.club/img/68/21257812/54162616.jpg','https://musify.club/release/the-sound-of-fractal-2020-1297232'),
 (8658,31,'Beatport Psy Trance. Electro Sound Pack #99','2020-06-25 00:00:00',0,'https://37s-a.musify.club/img/68/21458358/54549016.jpg','https://musify.club/release/beatport-psy-trance-electro-sound-pack-99-2020-1307834'),
 (8659,31,'Beatport Psy Trance. Electro Sound Pack #92 CD1','2020-05-30 00:00:00',0,'https://38s.musify.club/img/68/21218403/54098619.jpg','https://musify.club/release/beatport-psy-trance-electro-sound-pack-92-cd1-2020-1295615'),
 (8660,31,'Beatport Psychedelic Trance. Electro Sound Pack #115','2020-07-05 00:00:00',0,'https://38s.musify.club/img/68/21527067/54698410.jpg','https://musify.club/release/beatport-psychedelic-trance-electro-sound-pack-115-2020-1311909'),
 (8661,31,'Sounds Of New Generation 1','2019-12-06 00:00:00',0,'https://38s-a.musify.club/img/70/20090008/51703140.jpg','https://musify.club/release/sounds-of-new-generation-1-2019-1231490'),
 (8662,31,'New Habitat CD2','2020-03-16 00:00:00',0,'https://38s-a.musify.club/img/70/20626416/52877296.jpg','https://musify.club/release/new-habitat-cd2-2019-1262093'),
 (8663,31,'The Best Of Revealed Recordings 2019','2019-12-31 00:00:00',0,'https://39s.musify.club/img/69/20212754/51946583.jpg','https://musify.club/release/the-best-of-revealed-recordings-2019-2019-1236988'),
 (8664,31,'Hardwell On Air - Best Of June 2019 Pt. 1','2021-01-11 00:00:00',0,'https://38s.musify.club/img/68/22964019/58737957.jpg','https://musify.club/release/hardwell-on-air-best-of-june-2019-pt-1-2019-1427642'),
 (8665,31,'Hardwell On Air - Best Of June 2019 Pt. 2','2021-01-11 00:00:00',0,'https://38s.musify.club/img/68/22964025/58737963.jpg','https://musify.club/release/hardwell-on-air-best-of-june-2019-pt-2-2019-1427643'),
 (8666,31,'Hardwell On Air - Best Of June 2019 Pt. 3','2021-01-11 00:00:00',0,'https://38s.musify.club/img/68/22964043/58737969.jpg','https://musify.club/release/hardwell-on-air-best-of-june-2019-pt-3-2019-1427644'),
 (8667,31,'Atomic Pulse Archive','2019-10-11 00:00:00',0,'https://40s-a.musify.club/img/71/19732909/50961320.jpg','https://musify.club/release/atomic-pulse-archive-2019-1210756'),
 (8668,31,'Electro Panic [One Function Remix]','2019-04-10 00:00:00',0,'https://40s-a.musify.club/img/71/18543923/48524713.jpg','https://musify.club/release/electro-panic-one-function-remix-2019-1130412'),
 (8669,31,'Best Dj &quot; S World','2019-07-06 00:00:00',0,'https://40s-a.musify.club/img/71/19105760/49606333.jpg','https://musify.club/release/best-dj-s-world-2019-1167128'),
 (8670,31,'Dark Night','2019-08-02 00:00:00',0,'https://40s-a.musify.club/img/71/19280792/49986905.jpg','https://musify.club/release/dark-night-2019-1180174'),
 (8671,31,'Selection 2019','2020-01-05 00:00:00',0,'https://40s-a.musify.club/img/71/20224008/51978722.jpg','https://musify.club/release/selection-2019-2019-1237681'),
 (8672,31,'Club Sound Vol. 6','2019-11-22 00:00:00',0,'https://39s-a.musify.club/img/70/20039112/51591274.jpg','https://musify.club/release/club-sound-vol-6-2019-1228997'),
 (8673,31,'Part of the Dream [Compiled by Blastoyz &amp; Reality Test]','2019-11-19 00:00:00',0,'https://39s.musify.club/img/69/20025968/51553259.jpg','https://musify.club/release/part-of-the-dream-compiled-by-blastoyz-and-reality-test-2019-1227920'),
 (8674,31,'Essential Selection, vol. 1','2019-03-18 00:00:00',0,'https://39s-a.musify.club/img/70/18383412/48175810.jpg','https://musify.club/release/essential-selection-vol-1-2019-1119918'),
 (8675,31,'Goa, Vol. 69','2019-07-10 00:00:00',0,'https://40s-a.musify.club/img/71/19133130/49664573.jpg','https://musify.club/release/goa-vol-69-2019-1169072'),
 (8676,31,'Electronic Cosmic Party Part 1','2019-12-27 00:00:00',0,'https://39s-a.musify.club/img/70/20195377/51913054.jpg','https://musify.club/release/electronic-cosmic-party-part-1-2019-1236348'),
 (8677,31,'Club EDM Mysterious Vol.02 CD1','2020-03-02 00:00:00',0,'https://39s.musify.club/img/69/20659163/52867015.jpg','https://musify.club/release/club-edm-mysterious-vol-02-cd1-2019-1257389'),
 (8678,31,'New Habitat CD1','2020-03-16 00:00:00',0,'https://38s-a.musify.club/img/70/20626400/52877285.jpg','https://musify.club/release/new-habitat-cd1-2019-1262092'),
 (8679,31,'Monstercat - Best Of 2019','2020-11-20 00:00:00',0,'https://38s.musify.club/img/68/22587820/57946882.jpg','https://musify.club/release/monstercat-best-of-2019-2019-1403530'),
 (8680,31,'Monstercat Instinct Vol. 3','2020-07-24 00:00:00',0,'https://38s.musify.club/img/68/21661670/54976866.jpg','https://musify.club/release/monstercat-instinct-vol-3-2019-1322250'),
 (8681,31,'Chillout Moods Part 1','2019-06-07 00:00:00',0,'https://40s-a.musify.club/img/71/18893294/49212123.jpg','https://musify.club/release/chillout-moods-part-1-2019-1153718'),
 (8682,31,'Trance Top 1000 Chapter 5','2019-07-28 00:00:00',0,'https://40s-a.musify.club/img/71/19254524/49933771.jpg','https://musify.club/release/trance-top-1000-chapter-5-2019-1179037'),
 (8683,31,'Club Sound Vol.8','2020-02-29 00:00:00',0,'https://38s-a.musify.club/img/70/20539542/52671896.jpg','https://musify.club/release/club-sound-vol-8-2019-1257162'),
 (8684,31,'Do It','2019-06-10 00:00:00',0,'https://40s-a.musify.club/img/71/18943139/49261766.jpg','https://musify.club/release/do-it-2019-1155620'),
 (8685,31,'Monstercat Uncaged Vol. 6','2019-04-12 00:00:00',0,'https://38s-a.musify.club/img/71/18558655/48542722.jpg','https://musify.club/release/monstercat-uncaged-vol-6-2019-1131006'),
 (8686,31,'Bliss On Mushrooms','2018-11-09 00:00:00',0,'https://37s-a.musify.club/img/68/17521981/46149619.jpg','https://musify.club/release/bliss-on-mushrooms-2018-1066305'),
 (8687,31,'Guitarmass','2018-12-15 00:00:00',0,'https://37s.musify.club/img/69/17708577/46607916.jpg','https://musify.club/release/infected-mushroom-guitarmass-2018-1079049'),
 (8688,31,'Psyworld (2018)','2019-10-07 00:00:00',0,'https://40s-a.musify.club/img/71/19715864/50909865.jpg','https://musify.club/release/psyworld-2018-2018-1209431'),
 (8689,31,'Liquid Smoke [Shanti V Deedrah Remix]','2018-06-28 00:00:00',0,'https://38s-a.musify.club/img/70/16570523/44073712.jpg','https://musify.club/release/infected-mushroom-liquid-smoke-shanti-v-deedrah-remix-2018-1011910'),
 (8690,31,'IM21, pt. 1','2018-03-10 00:00:00',0,'https://41s-a.musify.club/img/70/15826318/42424910.jpg','https://musify.club/release/infected-mushroom-im21-pt-1-2018-969819'),
 (8691,31,'Electro Panic [Azax &amp; Boombastix Extended Remix]','2018-06-12 00:00:00',0,'https://37s.musify.club/img/69/16459021/43768129.jpg','https://musify.club/release/electro-panic-azax-and-boombastix-extended-remix-2018-1006634'),
 (8692,31,'Structural Mind Engine (2018)','2019-10-25 00:00:00',0,'https://38s.musify.club/img/69/19850442/51201353.jpg','https://musify.club/release/structural-mind-engine-2018-2018-1217872'),
 (8693,31,'Клубная Вечеринка CD2 ','2019-10-29 00:00:00',0,'https://38s-a.musify.club/img/70/19885457/51264235.jpg','https://musify.club/release/klubnaya-vecherinka-cd2-2018-1219370'),
 (8694,31,'Клубная Вечеринка Vol.3 CD1','2019-10-29 00:00:00',0,'https://40s-a.musify.club/img/71/19883312/51260011.jpg','https://musify.club/release/klubnaya-vecherinka-vol-3-cd1-2018-1219294'),
 (8695,31,'Head of NASA and the 2 Amish Boys','2018-12-15 00:00:00',0,'https://38s-a.musify.club/img/70/17712290/46631120.jpg','https://musify.club/release/head-of-nasa-and-the-2-amish-boys-2018-1079104'),
 (8696,31,'Psy-Nation Radio #007','2018-08-20 00:00:00',0,'https://37s.musify.club/img/69/16938051/44831773.jpg','https://musify.club/release/psy-nation-radio-007-2018-1032729'),
 (8697,31,'Psy-Nation Radio #012','2018-12-08 00:00:00',0,'https://40s.musify.club/img/69/17687906/46577370.jpg','https://musify.club/release/psy-nation-radio-012-2018-1077566'),
 (8698,31,'Клубная Вечеринка (CD1)','2018-09-10 00:00:00',0,'https://39s.musify.club/img/69/17088583/45187260.jpg','https://musify.club/release/klubnaya-vecherinka-cd1-2018-1042329'),
 (8699,31,'Psy Goa Trance. Beach Party','2019-02-18 00:00:00',0,'https://39s.musify.club/img/69/18178160/47732664.jpg','https://musify.club/release/psy-goa-trance-beach-party-2018-1107650'),
 (8700,31,'Goa World 2018.1','2019-02-21 00:00:00',0,'https://39s.musify.club/img/69/18182810/47738056.jpg','https://musify.club/release/goa-world-2018-1-2018-1108983'),
 (8701,31,'Monstercat - Best Of 2018','2018-12-15 00:00:00',0,'https://40s.musify.club/img/69/17723083/46777587.jpg','https://musify.club/release/monstercat-best-of-2018-2018-1079401'),
 (8702,31,'Mega Dance Summer 2018','2018-06-16 00:00:00',0,'https://37s-a.musify.club/img/68/16473634/43801424.jpg','https://musify.club/release/mega-dance-summer-2018-2018-1007049'),
 (8703,31,'Monstercat Uncaged Vol. 4','2019-08-02 00:00:00',0,'https://40s-a.musify.club/img/71/19301205/50011620.jpg','https://musify.club/release/monstercat-uncaged-vol-4-2018-1181208'),
 (8704,31,'Дискотека 2018 Dance Club Vol. 177 (CD1)','2018-05-05 00:00:00',0,'https://37s.musify.club/img/69/16204094/43214714.jpg','https://musify.club/release/diskoteka-2018-dance-club-vol-177-cd1-2018-991151'),
 (8705,31,'Digital Electro Ultimix','2018-12-30 00:00:00',0,'https://41s.musify.club/img/68/17813888/47008151.jpg','https://musify.club/release/digital-electro-ultimix-2018-1084926'),
 (8706,31,'The Metasphere','2020-03-24 00:00:00',0,'https://40s-a.musify.club/img/71/20791845/53152154.jpg','https://musify.club/release/the-metasphere-2018-1266032'),
 (8707,31,'Empire Records: Sound Clinic 3','2017-01-31 00:00:00',0,'https://41s.musify.club/img/69/12934521/33443662.jpg','https://musify.club/release/empire-records-sound-clinic-3-2017-809389'),
 (8708,31,'U R So Fucked (Riot Remix)','2019-12-17 00:00:00',0,'https://40s-a.musify.club/img/71/20151773/51823177.jpg','https://musify.club/release/infected-mushroom-u-r-so-fucked-riot-remix-2017-1234222'),
 (8709,31,'Hommega 20 Aniversary','2018-09-02 00:00:00',0,'https://38s-a.musify.club/img/70/16984464/45013914.jpg','https://musify.club/release/hommega-20-aniversary-2017-1038504'),
 (8710,31,'Spitfire','2018-01-21 00:00:00',0,'https://41s-a.musify.club/img/71/15564133/39252287.jpg','https://musify.club/release/infected-mushrooms-spitfire-2017-955263'),
 (8711,31,'Private Party Vol. 1','2017-01-31 00:00:00',0,'https://41s.musify.club/img/68/12932575/33442347.jpg','https://musify.club/release/private-party-vol-1-2017-809292'),
 (8712,31,'Euro Truck Simulator 2 Vol.6','2017-01-29 00:00:00',0,'https://41s.musify.club/img/69/12907185/33405897.jpg','https://musify.club/release/euro-truck-simulator-2-vol-6-2017-808205'),
 (8713,31,'The First Protocol','2017-05-22 00:00:00',0,'https://41s.musify.club/img/68/13837950/35545383.jpg','https://musify.club/release/the-first-protocol-2017-856692'),
 (8714,31,'Return To The Sauce','2017-01-27 00:00:00',0,'https://41s.musify.club/img/69/12900666/33375877.jpg','https://musify.club/release/return-to-the-sauce-2017-807638'),
 (8715,31,'Selection Of The Finest Psytrance Chapter 2','2017-09-22 00:00:00',0,'https://41s-a.musify.club/img/71/14663699/37360760.jpg','https://musify.club/release/selection-of-the-finest-psytrance-chapter-2-2017-908253'),
 (8716,31,'Selection Of The Finest Psytrance Chapter 1','2017-09-19 00:00:00',0,'https://41s-a.musify.club/img/71/14663693/37330165.jpg','https://musify.club/release/selection-of-the-finest-psytrance-chapter-1-2017-907714'),
 (8717,31,'The Shen','2018-01-21 00:00:00',0,'https://41s-a.musify.club/img/71/15564163/39252311.jpg','https://musify.club/release/infected-mushrooms-the-shen-2017-955260'),
 (8718,31,'Becoming Insane (Remix)','2018-01-21 00:00:00',0,'https://41s-a.musify.club/img/71/15566488/39256592.jpg','https://musify.club/release/becoming-insane-remix-2017-955325'),
 (8719,31,'Deep House Collection Vol. 100','2016-12-15 00:00:00',0,'https://41s.musify.club/img/68/12632430/32808813.jpg','https://musify.club/release/deep-house-collection-vol-100-2016-792450'),
 (8720,31,'Танцевальный рай: Only Hits','2016-12-15 00:00:00',0,'https://41s.musify.club/img/69/12626394/32805434.jpg','https://musify.club/release/tantsevalnii-rai-only-hits-2016-792305'),
 (8721,31,'Galgalatz Annual Parade 2016','2019-10-18 00:00:00',0,'https://39s-a.musify.club/img/70/19793389/51089577.jpg','https://musify.club/release/galgalatz-annual-parade-2016-2016-1214823'),
 (8722,31,'Shutafim Baam','2017-05-02 00:00:00',0,'https://41s.musify.club/img/68/13675584/35210404.jpg','https://musify.club/release/shutafim-baam-2016-848458'),
 (8723,31,'Old Is Gold','2016-02-09 00:00:00',0,'https://40s.musify.club/img/69/10847581/29022193.jpg','https://musify.club/release/old-is-gold-2016-691510'),
 (8724,31,'Psy-Progressive Trance Vol. 1','2017-01-05 00:00:00',0,'https://41s.musify.club/img/69/12764503/33082513.jpg','https://musify.club/release/psy-progressive-trance-vol-1-2016-799688'),
 (8725,31,'Танцевальный Рай 36','2016-01-24 00:00:00',0,'https://40s.musify.club/img/69/10738040/28749002.jpg','https://musify.club/release/tantsevalnii-rai-36-2016-682666'),
 (8726,31,'Fields Of Grey','2015-10-16 00:00:00',0,'https://39s.musify.club/img/68/10164646/26516811.jpg','https://musify.club/release/fields-of-grey-2015-654938'),
 (8727,31,'Goa Session by Ritmo','2015-08-28 00:00:00',0,'https://39s.musify.club/img/69/9980297/26089823.jpg','https://musify.club/release/goa-session-by-ritmo-2015-644340'),
 (8728,31,'Den Brysomme Mannen Vol. 1 – Monkey In The Vigeland&#39;s Park (Armandin V Project)','2018-01-27 00:00:00',0,'https://38s.musify.club/img/69/15581067/39361689.jpg','https://musify.club/release/den-brysomme-mannen-vol-1-monkey-in-the-vigelands-park-armandin-v-proj-2015-956817'),
 (8729,31,'Радио Аэростат Вып. 508-Новые Песни. Февраль','2015-04-13 00:00:00',0,'https://39s.musify.club/img/68/9276155/24617073.jpg','https://musify.club/release/radio-aerostat-vip-508-novie-pesni-fevral-2015-612793'),
 (8730,31,'Converting Vegetarians II','2015-09-14 00:00:00',0,'https://39s.musify.club/img/68/10029458/26211778.jpg','https://musify.club/release/infected-mushroom-converting-vegetarians-ii-2015-647078'),
 (8731,31,'Friends on Mushrooms','2015-01-05 00:00:00',0,'https://39s-a.musify.club/img/71/8772122/23394205.jpg','https://musify.club/release/infected-mushroom-friends-on-mushrooms-2015-587993'),
 (8732,31,'Super Hits Of Uplifting &amp; Vocal Trance (2015)','2017-04-16 00:00:00',0,'https://41s.musify.club/img/69/13552693/34955599.jpg','https://musify.club/release/super-hits-of-uplifting-and-vocal-trance-2015-2015-841747'),
 (8733,31,'A Decade Later','2017-06-28 00:00:00',0,'https://41s.musify.club/img/69/14022478/35975467.jpg','https://musify.club/release/a-decade-later-2015-867427'),
 (8734,31,'Global Trance Grooves 151 (13-10-2015) Javier Bussola Guestmix','2020-01-27 00:00:00',0,'https://38s.musify.club/img/69/20310249/52179529.jpg','https://musify.club/release/global-trance-grooves-151-13-10-2015-javier-bussola-guestmix-2015-1241512'),
 (8735,31,'От Севы Даб Часть 2','2015-11-08 00:00:00',0,'https://39s.musify.club/img/68/10297098/26724217.jpg','https://musify.club/release/ot-sevi-dab-chast-2-2015-660063'),
 (8736,31,'Top 100 DJ 2015','2018-05-01 00:00:00',0,'https://37s-a.musify.club/img/71/16164697/43161359.jpg','https://musify.club/release/top-100-dj-2015-2015-990026'),
 (8737,31,'Rock, Rock And Rock','2014-06-20 00:00:00',0,'https://40s-a.musify.club/img/70/7500252/21030686.jpg','https://musify.club/release/rock-rock-and-rock-2014-513554'),
 (8738,31,'Friends On Mushrooms, Vol. 3','2014-06-26 00:00:00',0,'https://37s-a.musify.club/img/71/7545959/21109745.jpg','https://musify.club/release/infected-mushroom-friends-on-mushrooms-vol-3-2014-515869'),
 (8739,31,'Electric Daisy Carnival 2014 Las Vegas (Day 3)','2014-09-10 00:00:00',0,'https://39s.musify.club/img/69/7900116/21926353.jpg','https://musify.club/release/electric-daisy-carnival-2014-las-vegas-day-3-2014-544326'),
 (8740,31,'Full On: Fluoro Vol 2','2014-02-02 00:00:00',0,'https://37s-a.musify.club/img/68/6630383/27797188.jpg','https://musify.club/release/full-on-fluoro-vol-2-2014-469489'),
 (8741,31,'Битва DJ','2014-07-01 00:00:00',0,'https://39s.musify.club/img/69/7561681/21162355.jpg','https://musify.club/release/bitva-dj-2014-517227'),
 (8742,31,'Клубные Новинки Vol. 470 (2014)','2014-12-05 00:00:00',0,'https://37s.musify.club/img/69/8564848/23057992.jpg','https://musify.club/release/klubnie-novinki-vol-470-2014-2014-578858'),
 (8743,31,'Sexy Trance #75','2016-09-26 00:00:00',0,'https://39s.musify.club/img/69/12138108/31779219.jpg','https://musify.club/release/sexy-trance-75-2013-766378'),
 (8744,31,'Trance Shivers Vol. 2','2014-02-19 00:00:00',0,'https://37s-a.musify.club/img/71/6726187/19469088.jpg','https://musify.club/release/trance-shivers-vol-2-2013-475272'),
 (8745,31,'Super Trance Collection CD 38','2014-04-07 00:00:00',0,'https://40s-a.musify.club/img/70/7014101/20036561.jpg','https://musify.club/release/super-trance-collection-cd-38-2013-489305'),
 (8746,31,'Enjoy The Sounds 20.5 All The Best','2016-09-18 00:00:00',0,'https://37s.musify.club/img/69/12104903/31691098.jpg','https://musify.club/release/enjoy-the-sounds-20-5-all-the-best-2013-764072'),
 (8747,31,'Friends On Mushrooms, Vol. 1,2','2013-08-04 00:00:00',0,'https://37s-a.musify.club/img/68/5371246/16285316.jpg','https://musify.club/release/infected-mushroom-friends-on-mushrooms-vol-1-2-2013-394495'),
 (8748,31,'See Me Now','2013-10-14 00:00:00',0,'https://39s-a.musify.club/img/70/5371330/17634334.jpg','https://musify.club/release/infected-mushroom-see-me-now-2013-431296'),
 (8749,31,'Sense Of Vocal Trance Volume 58','2014-01-19 00:00:00',0,'https://37s-a.musify.club/img/68/6486017/27847667.jpg','https://musify.club/release/sense-of-vocal-trance-volume-58-2013-466398'),
 (8750,31,'Sushi Volume 16','2014-01-12 00:00:00',0,'https://39s-a.musify.club/img/70/6445547/18948884.jpg','https://musify.club/release/sushi-volume-16-2013-463226'),
 (8751,31,'Trance Pro V.17','2013-02-26 00:00:00',0,'https://39s.musify.club/img/69/4711322/15493512.jpg','https://musify.club/release/trance-pro-v-17-2013-365620'),
 (8752,31,'Клубные Новинки 233','2014-03-26 00:00:00',0,'https://38s-a.musify.club/img/70/6918861/19857560.jpg','https://musify.club/release/klubnie-novinki-233-2013-485072'),
 (8753,31,'Omvision Pt. 8','2014-12-08 00:00:00',0,'https://39s.musify.club/img/68/8394226/23087553.jpg','https://musify.club/release/omvision-pt-8-2012-579729'),
 (8754,31,'Paul Oakenfold-Full On Fluoro 009 (24.01.2012)','2012-02-08 00:00:00',0,'https://39s.musify.club/img/69/3199973/13053548.jpg','https://musify.club/release/paul-oakenfold-full-on-fluoro-009-24-01-2012-2012-282403'),
 (8755,31,'Dance Planet [V.I.P.]','2012-04-13 00:00:00',0,'https://41s.musify.club/img/68/3426505/41331215.jpg','https://musify.club/release/dance-planet-v-i-p-2012-297435'),
 (8756,31,'Morning Becomes Eclectic','2017-02-10 00:00:00',0,'https://41s.musify.club/img/69/13013921/33615751.jpg','https://musify.club/release/infected-mushroom-morning-becomes-eclectic-2012-814000'),
 (8757,31,'U R So Smart','2012-05-02 00:00:00',0,'https://41s.musify.club/img/68/3488994/41300810.jpg','https://musify.club/release/infected-mushroom-u-r-so-smart-2012-301725'),
 (8758,31,'Nation Of Wusses','2012-05-02 00:00:00',0,'https://41s-a.musify.club/img/71/14816390/37640981.jpg','https://musify.club/release/infected-mushrooms-nation-of-wusses-2012-301723'),
 (8759,31,'Atid Matok','2017-10-14 00:00:00',0,'https://41s-a.musify.club/img/71/14861564/37749823.jpg','https://musify.club/release/atid-matok-2012-918534'),
 (8760,31,'Erotic Desires Volume 206','2012-07-11 00:00:00',0,'https://41s-a.musify.club/img/70/3717122/39932826.jpg','https://musify.club/release/erotic-desires-volume-206-2012-316555'),
 (8761,31,'Vocal Passion, Vol.28','2012-06-05 00:00:00',0,'https://41s.musify.club/img/68/3612146/41243506.jpg','https://musify.club/release/vocal-passion-vol-28-2012-309679'),
 (8762,31,'Army Of Mushrooms','2012-05-03 00:00:00',0,'https://41s.musify.club/img/68/3495636/41297747.jpg','https://musify.club/release/infected-mushroom-army-of-mushrooms-2012-302165'),
 (8763,31,'Vocal Trance Vol.34','2016-03-07 00:00:00',0,'https://40s.musify.club/img/69/11011786/29393746.jpg','https://musify.club/release/vocal-trance-vol-34-2012-702295'),
 (8764,31,'Army Of Mushrooms- Demo','2012-03-27 00:00:00',0,'https://41s.musify.club/img/68/3371394/41360066.jpg','https://musify.club/release/infected-mushroom-army-of-mushrooms-demo-2012-293951'),
 (8765,31,'A State Of Trance 550 Part 3 - Paul Oakenfold','2012-03-10 00:00:00',0,'https://39s.musify.club/img/69/3313260/15405274.jpg','https://musify.club/release/a-state-of-trance-550-part-3-paul-oakenfold-2012-289284'),
 (8766,31,'Evilution','2012-03-10 00:00:00',0,'https://39s.musify.club/img/69/3302738/15431338.jpg','https://musify.club/release/evilution-2012-289193'),
 (8767,31,'U R So Fucked','2012-05-02 00:00:00',0,'https://41s-a.musify.club/img/71/14816379/37640965.jpg','https://musify.club/release/infected-mushrooms-u-r-so-fucked-2012-301726'),
 (8768,31,'Pink Nightmares','2011-09-30 00:00:00',0,'https://41s-a.musify.club/img/70/1260402/40127632.jpg','https://musify.club/release/infected-mushroom-pink-nightmares-2011-248060'),
 (8769,31,'Paul Oakenfold-Full On Fluoro 008 (27.12.2011)','2012-02-08 00:00:00',0,'https://39s.musify.club/img/69/3199935/15405679.jpg','https://musify.club/release/paul-oakenfold-full-on-fluoro-008-27-12-2011-2011-282402'),
 (8770,31,'Live At Superclasico Tel Aviv (10-02-2011)','2017-02-10 00:00:00',0,'https://41s.musify.club/img/69/13013870/33615637.jpg','https://musify.club/release/live-at-superclasico-tel-aviv-10-02-2011-2011-814001'),
 (8771,31,'Paul Oakenfold-Full On Fluoro 005 (27.09.2011)','2012-02-08 00:00:00',0,'https://39s.musify.club/img/69/3197959/15401678.jpg','https://musify.club/release/paul-oakenfold-full-on-fluoro-005-27-09-2011-2011-282328'),
 (8772,31,'Paul Oakenfold-Full On Fluoro 007 (22.11.2011)','2012-02-08 00:00:00',0,'https://39s.musify.club/img/69/3198002/15420853.jpg','https://musify.club/release/paul-oakenfold-full-on-fluoro-007-22-11-2011-2011-282330'),
 (8773,31,'I&#39;m Alive','2012-02-07 00:00:00',0,'https://39s.musify.club/img/69/3192393/15460174.jpg','https://musify.club/release/im-alive-2011-282247'),
 (8774,31,'530-A State Of Trance (13.10.2011)','2012-01-20 00:00:00',0,'https://39s.musify.club/img/69/2942044/13021474.jpg','https://musify.club/release/530-a-state-of-trance-13-10-2011-2011-276679'),
 (8775,31,'We Are Planet Perfecto Volume 01','2011-12-08 00:00:00',0,'https://39s.musify.club/img/69/1846167/15393620.jpg','https://musify.club/release/we-are-planet-perfecto-volume-01-2011-265178'),
 (8776,31,'Trance Essentials 2012 Vol. 1 [CD 1]','2011-12-13 00:00:00',0,'https://39s.musify.club/img/69/1900489/15447625.jpg','https://musify.club/release/trance-essentials-2012-vol-1-cd-1-2011-266566'),
 (8777,31,'50 Tracks For You Car (CD 2)','2011-12-13 00:00:00',0,'https://39s.musify.club/img/69/1900106/15447612.jpg','https://musify.club/release/50-tracks-for-you-car-cd-2-2011-266539'),
 (8778,31,'Paul Oakenfold-Full On Fluoro 006 (25.10.2011)','2012-02-08 00:00:00',0,'https://39s.musify.club/img/69/3198058/15443940.jpg','https://musify.club/release/paul-oakenfold-full-on-fluoro-006-25-10-2011-2011-282332'),
 (8779,31,'Deck And Sheker','2017-10-14 00:00:00',0,'https://41s-a.musify.club/img/71/14861535/37749782.jpg','https://musify.club/release/infected-mushrooms-deck-and-sheker-2010-918529'),
 (8780,31,'One Day','2017-10-14 00:00:00',0,'https://41s-a.musify.club/img/71/14861542/37749792.jpg','https://musify.club/release/one-day-2010-918530'),
 (8781,31,'Killing Time__The Remixes','2017-10-14 00:00:00',0,'https://41s-a.musify.club/img/71/14861546/37749799.jpg','https://musify.club/release/killing-time-the-remixes-2010-918533'),
 (8782,31,'Trance In Motion Vol.50','2011-05-22 00:00:00',0,'https://41s-a.musify.club/img/70/1161528/40228278.jpg','https://musify.club/release/trance-in-motion-vol-50-2010-215125'),
 (8783,31,'The Goa Mix 2011 [CD1]','2010-11-23 00:00:00',0,'https://41s-a.musify.club/img/70/1064390/40326721.jpg','https://musify.club/release/the-goa-mix-2011-cd1-2010-107977'),
 (8784,31,'This Is Rave-Electro','2020-12-04 00:00:00',0,'https://38s.musify.club/img/68/22683996/58140272.jpg','https://musify.club/release/this-is-rave-electro-2010-1408988'),
 (8785,31,'Global Trance Grooves 078 (13-10-2009) Airwave Guestmix','2020-01-17 00:00:00',0,'https://38s.musify.club/img/69/20287155/52100387.jpg','https://musify.club/release/global-trance-grooves-078-13-10-2009-airwave-guestmix-2009-1240372'),
 (8786,31,'Heat Seekers Israeli Trance Allstars','2017-10-17 00:00:00',0,'https://41s-a.musify.club/img/71/14887235/37801946.jpg','https://musify.club/release/heat-seekers-israeli-trance-allstars-2009-919861'),
 (8787,31,'Psy-Trance Euphoria 2 [CD 3: The Fun Stuff!! (Full On)]','2016-04-07 00:00:00',0,'https://40s.musify.club/img/69/11211675/29797612.jpg','https://musify.club/release/psy-trance-euphoria-2-cd-3-the-fun-stuff-full-on-2009-713569'),
 (8788,31,'Smashing The Opponent (Remixes)','2017-10-14 00:00:00',0,'https://41s-a.musify.club/img/71/14861529/37749773.jpg','https://musify.club/release/infected-mushrooms-smashing-the-opponent-remixes-2009-918531'),
 (8789,31,'I Love Trance Paychedelic','2013-11-21 00:00:00',0,'https://39s-a.musify.club/img/70/6023838/18242825.jpg','https://musify.club/release/i-love-trance-paychedelic-2009-440119'),
 (8790,31,'Legend Of The Black Shawarma','2009-10-09 00:00:00',0,'https://41s-a.musify.club/img/70/865562/40526062.jpg','https://musify.club/release/infected-mushrooms-legend-of-the-black-shawarma-2009-44973'),
 (8791,31,'Smashing The Opponent','2009-08-27 00:00:00',0,'https://41s-a.musify.club/img/70/857402/40534808.jpg','https://musify.club/release/infected-mushroom-smashing-the-opponent-2009-42253'),
 (8792,31,'You Don&#39;t Mess with the Zohan - OST / Не шутите с Зоханом - Саундтрек','2010-12-16 00:00:00',0,'https://41s-a.musify.club/img/70/1133781/40256791.jpg','https://musify.club/release/you-dont-mess-with-the-zohan-ost-ne-shutite-s-zohanom-saundtrek-2008-205883'),
 (8793,31,'Live At Life Concert Hall Queretaro Mexico','2017-02-10 00:00:00',0,'https://41s.musify.club/img/69/13013795/33615552.jpg','https://musify.club/release/infected-mushroom-live-at-life-concert-hall-queretaro-mexico-2008-813998'),
 (8794,31,'Live@Kazantip&#39;16','2017-02-10 00:00:00',0,'https://41s.musify.club/img/69/13013810/33615567.jpg','https://musify.club/release/infected-mushroom-live-kazantip16-2008-813984'),
 (8795,31,'Multiverse','2017-05-04 00:00:00',0,'https://41s.musify.club/img/68/13682737/35250394.jpg','https://musify.club/release/multiverse-2008-849412'),
 (8796,31,'EA Sports UEFA EURO 2008 - OST','2008-11-01 00:00:00',0,'https://41s-a.musify.club/img/70/1120305/40270257.jpg','https://musify.club/release/ea-sports-uefa-euro-2008-ost-2008-201401'),
 (8797,31,'2Ch OST - CD1','2012-08-18 00:00:00',0,'https://39s.musify.club/img/69/3845059/15419947.jpg','https://musify.club/release/2ch-ost-cd1-2008-322685'),
 (8798,31,'Psychedelic Trance Vol.1','2015-04-29 00:00:00',0,'https://39s.musify.club/img/68/9385794/24912887.jpg','https://musify.club/release/psychedelic-trance-vol-1-2007-616599'),
 (8799,31,'10Th Anniversary Show [Live In Tel Aviv 22/11/2007 ]','2017-02-12 00:00:00',0,'https://41s.musify.club/img/68/13013719/33646528.jpg','https://musify.club/release/10th-anniversary-show-live-in-tel-aviv-22-11-2007-2007-814818'),
 (8800,31,'Live At Fist','2017-02-10 00:00:00',0,'https://41s.musify.club/img/69/13013749/33615519.jpg','https://musify.club/release/infected-mushroom-live-at-fist-2007-813987'),
 (8801,31,'Global Trance Grooves 047 (13-03-2007) Freq Guestmix','2020-01-17 00:00:00',0,'https://38s.musify.club/img/69/20281423/52094400.jpg','https://musify.club/release/global-trance-grooves-047-13-03-2007-freq-guestmix-2007-1240118'),
 (8802,31,'Retrodelic Vibes 3','2017-01-14 00:00:00',0,'https://41s.musify.club/img/68/12803305/33200586.jpg','https://musify.club/release/retrodelic-vibes-3-2007-802950'),
 (8803,31,'Vicious Delicious','2007-10-17 00:00:00',0,'https://41s-a.musify.club/img/70/706411/40682039.jpg','https://musify.club/release/infected-mushroom-vicious-delicious-2007-5666'),
 (8804,31,'Becoming Insane','2009-08-17 00:00:00',0,'https://41s-a.musify.club/img/70/854855/40537354.jpg','https://musify.club/release/infected-mushroom-becoming-insane-2007-41404'),
 (8805,31,'The Doors [Remix] [Unreleased LP]','2009-08-27 00:00:00',0,'https://41s-a.musify.club/img/70/857417/40534791.jpg','https://musify.club/release/infected-mushroom-the-doors-remix-unreleased-lp-2007-42258'),
 (8806,31,'Aderlass, Vol.5 [CD2]','2008-08-13 00:00:00',0,'https://41s-a.musify.club/img/70/1119504/40271062.jpg','https://musify.club/release/aderlass-vol-5-cd2-2007-201134'),
 (8807,31,'JJJ The Club FM','2017-02-10 00:00:00',0,'https://41s.musify.club/img/69/13013585/33615395.jpg','https://musify.club/release/infected-mushroom-jjj-the-club-fm-2006-813994'),
 (8808,31,'Global Trance Grooves 041 (12-09-2006) Yahel Guestmix','2020-01-17 00:00:00',0,'https://39s-a.musify.club/img/70/20278476/52091616.jpg','https://musify.club/release/global-trance-grooves-041-12-09-2006-yahel-guestmix-2006-1240025'),
 (8809,31,'Global Trance Grooves 039 (11-07-2006) Infected Mushroom Guestmix','2020-01-19 00:00:00',0,'https://39s-a.musify.club/img/70/20273815/52112977.jpg','https://musify.club/release/global-trance-grooves-039-11-07-2006-infected-mushroom-guestmix-2006-1240544'),
 (8810,31,'Mos Session','2017-02-10 00:00:00',0,'https://41s.musify.club/img/69/13013644/33615458.jpg','https://musify.club/release/infected-mushroom-mos-session-2006-813995'),
 (8811,31,'Live Mix 2005','2017-02-10 00:00:00',0,'https://41s.musify.club/img/69/13013543/33615367.jpg','https://musify.club/release/infected-mushroom-live-mix-2005-2005-813986'),
 (8812,31,'Live At PLAY.FM','2017-02-10 00:00:00',0,'https://41s.musify.club/img/69/13013349/33615129.jpg','https://musify.club/release/infected-mushroom-live-at-play-fm-2005-813982'),
 (8813,31,'Super Set','2015-03-10 00:00:00',0,'https://39s-a.musify.club/img/71/9080205/24250513.jpg','https://musify.club/release/super-set-2005-604121'),
 (8814,31,'Computech','2017-09-25 00:00:00',0,'https://41s-a.musify.club/img/71/14710869/37435794.jpg','https://musify.club/release/computech-2005-910496'),
 (8815,31,'Stretched','2009-08-27 00:00:00',0,'https://41s-a.musify.club/img/70/857426/40534782.jpg','https://musify.club/release/infected-mushroom-stretched-2005-42261'),
 (8816,31,'Global Trance Grooves 013 (11-05-2004) John 00 Fleming Part2','2020-01-13 00:00:00',0,'https://39s-a.musify.club/img/70/20259008/52052600.jpg','https://musify.club/release/global-trance-grooves-013-11-05-2004-john-00-fleming-part2-2004-1239361'),
 (8817,31,'Live In Tel-Aviv 28-10','2017-02-10 00:00:00',0,'https://41s.musify.club/img/69/13012946/33614704.jpg','https://musify.club/release/infected-mushroom-live-in-tel-aviv-28-10-2004-813959'),
 (8818,31,'Live At GLZ Studios Israel','2017-02-10 00:00:00',0,'https://41s.musify.club/img/69/13012831/33614564.jpg','https://musify.club/release/infected-mushroom-live-at-glz-studios-israel-2004-813958'),
 (8819,31,'Trance On Air 11-09','2017-02-10 00:00:00',0,'https://41s.musify.club/img/69/13013048/33614800.jpg','https://musify.club/release/trance-on-air-11-09-2004-813969'),
 (8820,31,'Live In Moscow 17.04.2004','2010-08-12 00:00:00',0,'https://41s.musify.club/img/69/13012875/33614620.jpg','https://musify.club/release/infected-mushroom-live-in-moscow-17-04-2004-2004-89797'),
 (8821,31,'IM the supervisor','2009-01-17 00:00:00',0,'https://38s.musify.club/img/69/5017132/13210315.jpg','https://musify.club/release/infected-mushroom-im-the-supervisor-2004-26392'),
 (8822,31,'Artificial Visualisation','2019-06-30 00:00:00',0,'https://40s-a.musify.club/img/71/19069718/49534154.jpg','https://musify.club/release/infected-mushroom-artificial-visualisation-2004-1164727'),
 (8823,31,'Cities Of The Future','2008-08-25 00:00:00',0,'https://41s-a.musify.club/img/70/789584/40602712.jpg','https://musify.club/release/infected-mushroom-cities-of-the-future-2004-19646'),
 (8824,31,'Live At Dinamo Dvash','2017-02-10 00:00:00',0,'https://41s.musify.club/img/69/13012086/33614037.jpg','https://musify.club/release/infected-mushroom-live-at-dinamo-dvash-2003-813935'),
 (8825,31,'Open Air - The Best Of Euphoric Goa Trance Season 2003 [CD1]','2018-12-23 00:00:00',0,'https://40s.musify.club/img/69/17751994/46921565.jpg','https://musify.club/release/open-air-the-best-of-euphoric-goa-trance-season-2003-cd1-2003-1082687'),
 (8826,31,'Israliens 4: Solaris','2016-01-31 00:00:00',0,'https://40s.musify.club/img/69/10720542/28888370.jpg','https://musify.club/release/israliens-4-solaris-2003-687678'),
 (8827,31,'Life Is... Creation','2020-09-21 00:00:00',0,'https://38s.musify.club/img/68/22129823/57383840.jpg','https://musify.club/release/life-is-creation-2003-1377240'),
 (8828,31,'Raving Memories','2017-09-25 00:00:00',0,'https://41s-a.musify.club/img/71/14711429/37436432.jpg','https://musify.club/release/raving-memories-2003-910492'),
 (8829,31,'Songs From The Other Side','2009-08-27 00:00:00',0,'https://41s-a.musify.club/img/70/857540/40534668.jpg','https://musify.club/release/infected-mushroom-songs-from-the-other-side-2003-42299'),
 (8830,31,'Deeply Disturbed','2009-08-27 00:00:00',0,'https://41s-a.musify.club/img/70/857420/40534788.jpg','https://musify.club/release/infected-mushroom-deeply-disturbed-2003-42259'),
 (8831,31,'Global Trance Grooves 001 (13-05-2003) Astrix Guestmix','2020-01-12 00:00:00',0,'https://38s.musify.club/img/69/20254521/52045938.jpg','https://musify.club/release/global-trance-grooves-001-13-05-2003-astrix-guestmix-2003-1239196'),
 (8832,31,'Converting Vegetarians: CD1 Trance Side','2009-11-26 00:00:00',0,'https://40s-a.musify.club/img/70/26081229/65180188.jpg','https://musify.club/release/infected-mushroom-converting-vegetarians-cd1-trance-side-2003-52950'),
 (8833,31,'Converting Vegetarians: CD2 The Other Side','2009-11-26 00:00:00',0,'https://41s-a.musify.club/img/71/26082221/65181320.jpg','https://musify.club/release/infected-mushroom-converting-vegetarians-cd2-the-other-side-2003-52959'),
 (8834,31,'Goa Vol. 2 (CD2)','2016-06-10 00:00:00',0,'https://38s-a.musify.club/img/70/11552379/30524422.jpg','https://musify.club/release/goa-vol-2-cd2-2002-733540'),
 (8835,31,'Live At Japan','2017-02-10 00:00:00',0,'https://41s.musify.club/img/69/13012791/33614515.jpg','https://musify.club/release/live-at-japan-2002-813955'),
 (8836,31,'Adrenalin - Take One','2013-09-12 00:00:00',0,'https://40s.musify.club/img/69/5085254/17170548.jpg','https://musify.club/release/adrenalin-take-one-2002-411207'),
 (8837,31,'Psy Live Mix','2017-02-10 00:00:00',0,'https://41s.musify.club/img/69/13012748/33614461.jpg','https://musify.club/release/psy-live-mix-2002-813960'),
 (8838,31,'Live At DNA Lounge (9/14/02)','2017-02-10 00:00:00',0,'https://41s.musify.club/img/69/13012100/33614044.jpg','https://musify.club/release/infected-mushroom-live-at-dna-lounge-9-14-02-2002-813944'),
 (8839,31,'Live In Eilat Desert Israel','2017-02-10 00:00:00',0,'https://41s.musify.club/img/69/13012651/33614383.jpg','https://musify.club/release/live-in-eilat-desert-israel-2002-813951'),
 (8840,31,'Live At Brotherhood Beach Party In Tel Aviv','2017-02-10 00:00:00',0,'https://41s.musify.club/img/69/13012041/33614025.jpg','https://musify.club/release/infected-mushroom-live-at-brotherhood-beach-party-in-tel-aviv-2002-813936'),
 (8841,31,'Another Life...','2020-09-20 00:00:00',0,'https://38s.musify.club/img/68/22129681/57383617.jpg','https://musify.club/release/another-life-2002-1377199'),
 (8842,31,'Code:[604] Goa Trance','2019-03-08 00:00:00',0,'https://39s.musify.club/img/69/18300327/48012296.jpg','https://musify.club/release/code-604-goa-trance-2002-1115607'),
 (8843,31,'Birthday','2009-08-27 00:00:00',0,'https://41s-a.musify.club/img/70/857429/40534779.jpg','https://musify.club/release/infected-mushroom-birthday-2002-42262'),
 (8844,31,'Shpongle (Live In Eilat Desert)','2017-02-10 00:00:00',0,'https://41s.musify.club/img/69/13012458/33614160.jpg','https://musify.club/release/infected-mushroom-shpongle-live-in-eilat-desert-2002-813952'),
 (8845,31,'Outdoor Party Vol.III','2017-02-10 00:00:00',0,'https://41s.musify.club/img/69/13012009/33613992.jpg','https://musify.club/release/infected-mushroom-outdoor-party-vol-iii-2001-813933'),
 (8846,31,'Tribal Dance Experience','2018-08-31 00:00:00',0,'https://38s-a.musify.club/img/70/17007713/45045670.jpg','https://musify.club/release/tribal-dance-experience-2001-1037586'),
 (8847,31,'My Mummy Said','2018-07-09 00:00:00',0,'https://39s.musify.club/img/69/16651893/44211260.jpg','https://musify.club/release/infected-mushroom-my-mummy-said-2001-1017155'),
 (8848,31,'Virtual Trance Vol. 2. Digital Alchemy','2012-12-01 00:00:00',0,'https://37s-a.musify.club/img/68/5421002/16319164.jpg','https://musify.club/release/virtual-trance-vol-2-digital-alchemy-2001-346860'),
 (8849,31,'Mixing In Action','2008-11-02 00:00:00',0,'https://37s.musify.club/img/69/9134739/24299070.jpg','https://musify.club/release/mixing-in-action-2001-22922'),
 (8850,31,'B.P. Empire','2009-01-17 00:00:00',0,'https://41s-a.musify.club/img/70/809828/40582564.jpg','https://musify.club/release/infected-mushroom-b-p-empire-2001-26395'),
 (8851,31,'Classical Mushroom','2009-08-27 00:00:00',0,'https://41s-a.musify.club/img/70/857435/40534773.jpg','https://musify.club/release/infected-mushroom-classical-mushroom-2001-42264'),
 (8852,31,'B.P. Empire','2009-08-27 00:00:00',0,'https://41s-a.musify.club/img/70/857456/40534752.jpg','https://musify.club/release/infected-mushroom-b-p-empire-2001-42271'),
 (8853,31,'Goa Mix Volume 2','2018-08-28 00:00:00',0,'https://38s-a.musify.club/img/70/16984451/45013898.jpg','https://musify.club/release/goa-mix-volume-2-2000-1036314'),
 (8854,31,'Classical mushroom','2009-01-17 00:00:00',0,'https://41s-a.musify.club/img/70/809810/40582582.jpg','https://musify.club/release/infected-mushroom-classical-mushroom-2000-26389'),
 (8855,31,'Future Navigators II - The Second Tour','2015-04-30 00:00:00',0,'https://39s.musify.club/img/69/9400661/24922587.jpg','https://musify.club/release/future-navigators-ii-the-second-tour-2000-616834'),
 (8856,31,'Singles &amp; Remixes','2017-03-11 00:00:00',0,'https://41s.musify.club/img/69/13259589/34138392.jpg','https://musify.club/release/infected-mushrooms-singles-and-remixes-2000-827372'),
 (8857,31,'Infected Mushroom - Compilations Tracks','2020-05-15 00:00:00',0,'https://39s-a.musify.club/img/71/21113108/53872109.jpg','https://musify.club/release/infected-mushroom-infected-mushroom-compilations-tracks-2000-1290362'),
 (8858,31,'Live On Radio 99 Esc 07-03','2017-02-10 00:00:00',0,'https://41s.musify.club/img/69/13011947/33613913.jpg','https://musify.club/release/live-on-radio-99-esc-07-03-1999-813950'),
 (8859,31,'Israliens: Futuristic Psy-Trance For The Year 2000','2018-08-28 00:00:00',0,'https://38s-a.musify.club/img/70/16984456/45013906.jpg','https://musify.club/release/israliens-futuristic-psy-trance-for-the-year-2000-1999-1036315'),
 (8860,31,'The Gathering','2009-11-26 00:00:00',0,'https://41s-a.musify.club/img/70/889472/40502171.jpg','https://musify.club/release/infected-mushroom-the-gathering-1999-52948'),
 (8861,31,'The Digital Dance of Shiva','2008-07-14 00:00:00',0,'https://41s-a.musify.club/img/70/1119222/40271344.jpg','https://musify.club/release/the-digital-dance-of-shiva-1999-201040'),
 (8862,31,'Intelligate','2009-08-27 00:00:00',0,'https://41s-a.musify.club/img/70/857453/40534755.jpg','https://musify.club/release/infected-mushroom-intelligate-1999-42270'),
 (8863,31,'Full On Vol.2 - The Israeli DAT Mafia','2018-08-28 00:00:00',0,'https://38s-a.musify.club/img/70/16985415/45015183.jpg','https://musify.club/release/full-on-vol-2-the-israeli-dat-mafia-1998-1036317'),
 (8864,31,'Forest Of The Saints','2012-09-12 00:00:00',0,'https://39s.musify.club/img/69/3932644/15416813.jpg','https://musify.club/release/forest-of-the-saints-1998-327757'),
 (8865,31,'Psy-Trance Euphoria (Disc 2: The &#39;Full-On&#39; Mix)','2016-04-19 00:00:00',0,'https://38s-a.musify.club/img/70/11270340/29935543.jpg','https://musify.club/release/psy-trance-euphoria-disc-2-the-full-on-mix-717457'),
 (8866,36,'Argent Moon','2021-10-12 00:00:00',0,'https://37s-a.musify.club/img/71/24661147/62339120.jpg','https://musify.club/release/insomnium-argent-moon-2021-1545350'),
 (8867,36,'The Reticent','2021-05-24 00:00:00',0,'https://39s.musify.club/img/68/23933447/60763537.jpg','https://musify.club/release/insomnium-the-reticent-2021-1496811'),
 (8868,36,'The Antagonist','2021-07-17 00:00:00',0,'https://38s.musify.club/img/68/24272963/61485895.jpg','https://musify.club/release/insomnium-the-antagonist-2021-1522320'),
 (8869,36,'The Conjurer','2021-03-20 00:00:00',0,'https://38s-a.musify.club/img/71/23448580/59751801.jpg','https://musify.club/release/insomnium-the-conjurer-2021-1464945'),
 (8870,36,'Best Metal Of All Time','2020-04-15 00:00:00',0,'https://40s-a.musify.club/img/71/20872271/53361093.jpg','https://musify.club/release/best-metal-of-all-time-2020-1276101'),
 (8871,36,'Heart Like A Grave','2019-10-03 00:00:00',0,'https://40s-a.musify.club/img/71/19691774/50836895.jpg','https://musify.club/release/insomnium-heart-like-a-grave-2019-1206383'),
 (8872,36,'Valediction','2019-09-04 00:00:00',0,'https://40s.musify.club/img/68/23667907/60196365.jpg','https://musify.club/release/insomnium-valediction-2019-1192264'),
 (8873,36,'Pale Morning Star','2021-04-15 00:00:00',0,'https://39s.musify.club/img/68/23667892/60205780.jpg','https://musify.club/release/insomnium-pale-morning-star-2019-1479376'),
 (8874,36,'VA - Metal Compilation - Vol. 16 (CD4)','2017-12-06 00:00:00',0,'https://41s-a.musify.club/img/71/15280700/38637016.jpg','https://musify.club/release/va-metal-compilation-vol-16-cd4-2017-940164'),
 (8875,36,'Winter&#39;s Gate','2016-09-22 00:00:00',0,'https://39s.musify.club/img/69/12126566/31737875.jpg','https://musify.club/release/insomnium-winters-gate-2016-765265'),
 (8876,36,'Melodic Darkness Metal Vol. 1','2016-09-12 00:00:00',0,'https://37s.musify.club/img/69/12066447/31604369.jpg','https://musify.club/release/melodic-darkness-metal-vol-1-2016-761432'),
 (8877,36,'ULTRAMETAL Compilacia 2016','2016-11-09 00:00:00',0,'https://41s.musify.club/img/69/12409645/32358738.jpg','https://musify.club/release/ultrametal-compilacia-2016-2016-780910'),
 (8878,36,'Circle Of Silence','2016-12-09 00:00:00',0,'https://40s-a.musify.club/img/71/19066667/49482546.jpg','https://musify.club/release/circle-of-silence-2014-790742'),
 (8879,36,'Shadows Of The Dying Sun','2014-04-23 00:00:00',0,'https://40s-a.musify.club/img/70/7127789/20258153.jpg','https://musify.club/release/insomnium-shadows-of-the-dying-sun-2014-495030'),
 (8880,36,'While We Sleep','2014-04-11 00:00:00',0,'https://40s-a.musify.club/img/70/7040795/20088031.jpg','https://musify.club/release/insomnium-while-we-sleep-2014-490616'),
 (8881,36,'Revelations','2014-03-24 00:00:00',0,'https://38s-a.musify.club/img/70/6907683/19835067.jpg','https://musify.club/release/insomnium-revelations-2014-484411'),
 (8882,36,'Ephemeral','2013-09-26 00:00:00',0,'https://39s-a.musify.club/img/70/5587670/27816064.jpg','https://musify.club/release/insomnium-ephemeral-2013-426223'),
 (8883,36,'MDM Compilation (CD2 - Modern Era)','2011-12-31 00:00:00',0,'https://41s-a.musify.club/img/70/2010115/39640082.jpg','https://musify.club/release/mdm-compilation-cd2-modern-era-2011-269925'),
 (8884,36,'Kaikkien Aikojen Suomimetallit','2020-06-24 00:00:00',0,'https://39s-a.musify.club/img/70/21454565/54541248.jpg','https://musify.club/release/kaikkien-aikojen-suomimetallit-2011-1307718'),
 (8885,36,'Finnish Metal Mania 2','2020-06-23 00:00:00',0,'https://38s.musify.club/img/69/21442377/54518114.jpg','https://musify.club/release/finnish-metal-mania-2-2011-1307140'),
 (8886,36,'Weather The Storm','2011-04-29 00:00:00',0,'https://41s-a.musify.club/img/70/1149849/40240571.jpg','https://musify.club/release/insomnium-weather-the-storm-2011-211236'),
 (8887,36,'One For Sorrow','2011-10-30 00:00:00',0,'https://41s-a.musify.club/img/70/1278270/40109659.jpg','https://musify.club/release/insomnium-one-for-sorrow-2011-254019'),
 (8888,36,'Finland Melodic Death Metal Vol.1','2011-09-20 00:00:00',0,'https://41s-a.musify.club/img/70/1253589/40134476.jpg','https://musify.club/release/finland-melodic-death-metal-vol-1-2010-245790'),
 (8889,36,'Northern Warriors - Compilation XII: A Call To Arms','2011-12-07 00:00:00',0,'https://39s.musify.club/img/69/1828212/15432163.jpg','https://musify.club/release/northern-warriors-compilation-xii-a-call-to-arms-2009-264099'),
 (8890,36,'Where The Last Wave Broke','2010-05-22 00:00:00',0,'https://41s-a.musify.club/img/70/956048/40435421.jpg','https://musify.club/release/insomnium-where-the-last-wave-broke-2009-75142'),
 (8891,36,'Orbit Dance Vol.1','2011-09-21 00:00:00',0,'https://41s-a.musify.club/img/70/1254528/40133534.jpg','https://musify.club/release/orbit-dance-vol-1-2009-246103'),
 (8892,36,'Across The Dark','2009-11-09 00:00:00',0,'https://41s-a.musify.club/img/70/881243/40510379.jpg','https://musify.club/release/insomnium-across-the-dark-2009-50205'),
 (8893,36,'Finnish Melodic Death Metal [CD1]','2010-09-12 00:00:00',0,'https://41s-a.musify.club/img/70/1131879/40258702.jpg','https://musify.club/release/finnish-melodic-death-metal-cd1-2007-205253'),
 (8894,36,'Above the Weeping World','2009-06-08 00:00:00',0,'https://41s-a.musify.club/img/70/837686/40554490.jpg','https://musify.club/release/insomnium-above-the-weeping-world-2006-35679'),
 (8895,36,'Since the Day It All Came Down','2008-10-19 00:00:00',0,'https://41s-a.musify.club/img/70/797915/40594461.jpg','https://musify.club/release/insomnium-since-the-day-it-all-came-down-2004-22424'),
 (8896,36,'In The Halls Of Awaiting','2009-06-08 00:00:00',0,'https://41s-a.musify.club/img/70/837662/40554514.jpg','https://musify.club/release/insomnium-in-the-halls-of-awaiting-2002-35671'),
 (8897,36,'Underneath The Moonlight Waves','2010-03-22 00:00:00',0,'https://41s-a.musify.club/img/70/917741/40473957.jpg','https://musify.club/release/insomnium-underneath-the-moonlight-waves-2000-62371'),
 (8898,36,'Demo','2010-03-24 00:00:00',0,'https://41s-a.musify.club/img/70/923885/40467556.jpg','https://musify.club/release/insomnium-demo-1999-64419'),
 (8899,35,'Silence 3','2022-01-19 00:00:00',0,'https://40s-a.musify.club/img/70/25106617/62985725.jpg','https://musify.club/release/e-mantra-silence-3-2022-1566711'),
 (8900,35,'Beatport Synth Electronic: Sound Pack #406','2022-02-19 00:00:00',0,'https://41s.musify.club/img/68/25274848/63452532.jpg','https://musify.club/release/beatport-synth-electronic-sound-pack-406-2022-1584370'),
 (8901,35,'Hallucinogenic Trance','2022-05-25 00:00:00',0,'https://38s.musify.club/img/68/25805281/64693461.jpg','https://musify.club/release/hallucinogenic-trance-2022-1624502'),
 (8902,35,'Spacechillers Vol. 4','2021-04-19 00:00:00',0,'https://38s-a.musify.club/img/71/23690256/60269119.jpg','https://musify.club/release/spacechillers-vol-4-2021-1481770'),
 (8903,35,'Raining Lights','2021-05-11 00:00:00',0,'https://39s.musify.club/img/68/23842640/60572298.jpg','https://musify.club/release/e-mantra-raining-lights-2021-1490959'),
 (8904,35,'Spirit Catcher','2022-01-19 00:00:00',0,'https://38s.musify.club/img/68/24907841/62977788.jpg','https://musify.club/release/e-mantra-spirit-catcher-2021-1566300'),
 (8905,35,'Diary Of A Restless Mind','2021-02-16 00:00:00',0,'https://40s.musify.club/img/68/23221379/59269590.jpg','https://musify.club/release/diary-of-a-restless-mind-2021-1447060'),
 (8906,35,'E-Mantra / Kapnobatay / Night Hex','2021-07-01 00:00:00',0,'https://38s-a.musify.club/img/71/24193557/61310592.jpg','https://musify.club/release/e-mantra-kapnobatay-night-hex-2021-1516240'),
 (8907,35,'The Kite','2021-07-21 00:00:00',0,'https://40s.musify.club/img/69/24287702/61531957.jpg','https://musify.club/release/the-kite-2021-1523569'),
 (8908,35,'The Magic Of The Yellow Day','2021-04-07 00:00:00',0,'https://39s.musify.club/img/68/23590434/60031180.jpg','https://musify.club/release/the-magic-of-the-yellow-day-2021-1472989'),
 (8909,35,'Oceanica','2021-04-03 00:00:00',0,'https://38s.musify.club/img/68/23536404/59952931.jpg','https://musify.club/release/oceanica-2021-1471234'),
 (8910,35,'Frost','2021-03-05 00:00:00',0,'https://38s.musify.club/img/68/23353880/59548625.jpg','https://musify.club/release/frost-2021-1456534'),
 (8911,35,'Ambient Oceania','2021-03-06 00:00:00',0,'https://38s.musify.club/img/68/23344740/59560834.jpg','https://musify.club/release/ambient-oceania-2021-1457118'),
 (8912,35,'Hermetika','2021-01-10 00:00:00',0,'https://38s-a.musify.club/img/71/22966229/58721031.jpg','https://musify.club/release/e-mantra-hermetika-2021-1427085'),
 (8913,35,'Goa Trance 2021 Top 100 Hits DJ Mix Pt 1','2021-02-11 00:00:00',0,'https://38s.musify.club/img/68/23163789/59161747.jpg','https://musify.club/release/goa-trance-2021-top-100-hits-dj-mix-pt-1-2021-1442488'),
 (8914,35,'Last Transmission','2021-07-08 00:00:00',0,'https://38s.musify.club/img/68/24232898/61398049.jpg','https://musify.club/release/e-mantra-last-transmission-2021-1520067'),
 (8915,35,'Classy Psy Trance. White Session','2021-07-20 00:00:00',0,'https://40s.musify.club/img/69/24288991/61521074.jpg','https://musify.club/release/classy-psy-trance-white-session-2021-1523215'),
 (8916,35,'Uplifting Trance KM-2','2020-05-31 00:00:00',0,'https://38s-a.musify.club/img/70/21233685/54122701.jpg','https://musify.club/release/uplifting-trance-km-2-2020-1296422'),
 (8917,35,'Dissolved Forms. Psy Trance Collection CD2','2020-03-20 00:00:00',0,'https://40s-a.musify.club/img/71/20674529/52956064.jpg','https://musify.club/release/dissolved-forms-psy-trance-collection-cd2-2020-1264539'),
 (8918,35,'Top 100 Chillout Tracks Vol. 3','2020-03-15 00:00:00',0,'https://38s.musify.club/img/69/20610633/52854808.jpg','https://musify.club/release/top-100-chillout-tracks-vol-3-2020-1261150'),
 (8919,35,'Erotic Lounge','2020-11-04 00:00:00',0,'https://38s.musify.club/img/68/22454160/57730285.jpg','https://musify.club/release/erotic-lounge-2020-1396249'),
 (8920,35,'15 Years of Music Special Release, vol. 1','2020-08-02 00:00:00',0,'https://38s-a.musify.club/img/70/21743699/55464586.jpg','https://musify.club/release/e-mantra-15-years-of-music-special-release-vol-1-2020-1330279'),
 (8921,35,'Progressive Goa Trance 2020 Top 100 Hits DJ Mix','2020-09-10 00:00:00',0,'https://39s.musify.club/img/68/22012292/56539125.jpg','https://musify.club/release/progressive-goa-trance-2020-top-100-hits-dj-mix-2020-1373573'),
 (8922,35,'15 Years of Music Special Release, vol. 2','2020-08-25 00:00:00',0,'https://40s-a.musify.club/img/71/21883702/55817866.jpg','https://musify.club/release/e-mantra-15-years-of-music-special-release-vol-2-2020-1356339'),
 (8923,35,'Silence (Remastered)','2020-04-12 00:00:00',0,'https://39s.musify.club/img/69/20848926/53317899.jpg','https://musify.club/release/silence-remastered-2020-1274597'),
 (8924,35,'The Call of Goa, vol. 4','2020-11-08 00:00:00',0,'https://38s.musify.club/img/68/22490244/57773443.jpg','https://musify.club/release/the-call-of-goa-vol-4-2020-1397785'),
 (8925,35,'Drifting','2020-02-11 00:00:00',0,'https://39s-a.musify.club/img/71/20418747/52438624.jpg','https://musify.club/release/e-mantra-drifting-2020-1250398'),
 (8926,35,'15 Years of Music Special Release, vol. 3','2020-10-03 00:00:00',0,'https://40s.musify.club/img/68/22235394/57499310.jpg','https://musify.club/release/e-mantra-15-years-of-music-special-release-vol-3-2020-1383277'),
 (8927,35,'Goa Trance 2020, vol. 1','2020-02-09 00:00:00',0,'https://39s-a.musify.club/img/70/20370785/52396582.jpg','https://musify.club/release/goa-trance-2020-vol-1-2020-1248792'),
 (8928,35,'Silence 2','2020-12-05 00:00:00',0,'https://38s.musify.club/img/68/22691282/58157584.jpg','https://musify.club/release/e-mantra-silence-2-2020-1409445'),
 (8929,35,'Floating Spirals IV','2020-09-12 00:00:00',0,'https://40s.musify.club/img/68/22047004/57318170.jpg','https://musify.club/release/floating-spirals-iv-2020-1374435'),
 (8930,35,'Night Guardian','2020-05-15 00:00:00',0,'https://39s.musify.club/img/68/21106538/53861395.jpg','https://musify.club/release/e-mantra-night-guardian-2020-1289914'),
 (8931,35,'Ovnimoon Trax Selection Part 2','2020-05-20 00:00:00',0,'https://40s-a.musify.club/img/71/21142089/53949245.jpg','https://musify.club/release/ovnimoon-trax-selection-part-2-2020-1292099'),
 (8932,35,'Symphonix Psychedelic Goa Trance','2020-03-16 00:00:00',0,'https://39s.musify.club/img/69/20631751/52884650.jpg','https://musify.club/release/symphonix-psychedelic-goa-trance-2019-1262437'),
 (8933,35,'Goa Moon, vol. 10','2019-06-17 00:00:00',0,'https://40s-a.musify.club/img/71/18990528/49360598.jpg','https://musify.club/release/goa-moon-vol-10-2019-1158644'),
 (8934,35,'Liquid Chill Vol. 03','2020-07-31 00:00:00',0,'https://39s-a.musify.club/img/71/21728757/55378067.jpg','https://musify.club/release/liquid-chill-vol-03-2019-1328388'),
 (8935,35,'Reflected Solstice','2019-06-23 00:00:00',0,'https://40s-a.musify.club/img/71/19028495/49440701.jpg','https://musify.club/release/reflected-solstice-2019-1161411'),
 (8936,35,'Abstrct Vision: Psychedelic Trance','2019-12-06 00:00:00',0,'https://39s-a.musify.club/img/70/20092714/51705468.jpg','https://musify.club/release/abstrct-vision-psychedelic-trance-2019-1231591'),
 (8937,35,'Goa 2020 Top 40 Hits','2019-12-17 00:00:00',0,'https://38s-a.musify.club/img/70/20140496/51811186.jpg','https://musify.club/release/goa-2020-top-40-hits-2019-1233761'),
 (8938,35,'Resonant Reality. Trance Psychedelic Party','2019-09-15 00:00:00',0,'https://40s-a.musify.club/img/71/19569464/50592477.jpg','https://musify.club/release/resonant-reality-trance-psychedelic-party-2019-1199045'),
 (8939,35,'Tartarus','2019-02-12 00:00:00',0,'https://37s-a.musify.club/img/71/18126426/47666251.jpg','https://musify.club/release/e-mantra-tartarus-2019-1104699'),
 (8940,35,'Underground Psy-Trance Anthems, Vol. 10','2019-09-01 00:00:00',0,'https://40s-a.musify.club/img/71/19436358/50366352.jpg','https://musify.club/release/underground-psy-trance-anthems-vol-10-2019-1190362'),
 (8941,35,'Music Of The Future 2019','2019-07-11 00:00:00',0,'https://40s-a.musify.club/img/71/19151781/49688325.jpg','https://musify.club/release/music-of-the-future-2019-2019-1170027'),
 (8942,35,'Soundscapes','2019-11-29 00:00:00',0,'https://39s.musify.club/img/69/20055730/51642700.jpg','https://musify.club/release/e-mantra-soundscapes-2019-1229958'),
 (8943,35,'Chillout Moods Part 1','2019-06-07 00:00:00',0,'https://40s-a.musify.club/img/71/18893294/49212123.jpg','https://musify.club/release/chillout-moods-part-1-2019-1153718'),
 (8944,35,'Wind Of Buri - Cities In The Clouds 130 (Part 2)','2020-01-28 00:00:00',0,'https://38s.musify.club/img/69/20339346/52199648.jpg','https://musify.club/release/wind-of-buri-cities-in-the-clouds-130-part-2-2018-1242243'),
 (8945,35,'Time Paradox','2020-03-24 00:00:00',0,'https://39s.musify.club/img/69/20736234/53045793.jpg','https://musify.club/release/time-paradox-2018-1266043'),
 (8946,35,'Goa Trance Aural Expansion v.2','2018-04-17 00:00:00',0,'https://41s-a.musify.club/img/71/16078370/42965918.jpg','https://musify.club/release/goa-trance-aural-expansion-v-2-2018-984720'),
 (8947,35,'Signals from the Surface 2','2018-09-08 00:00:00',0,'https://37s-a.musify.club/img/71/17072948/45143891.jpg','https://musify.club/release/signals-from-the-surface-2-2018-1040917'),
 (8948,35,'Colors of Goa, v.3','2018-09-08 00:00:00',0,'https://40s-a.musify.club/img/70/17070351/45141468.jpg','https://musify.club/release/colors-of-goa-v-3-2018-1040858'),
 (8949,35,'Progress to Goa, vol.3','2018-12-08 00:00:00',0,'https://40s.musify.club/img/69/17688696/46578255.jpg','https://musify.club/release/progress-to-goa-vol-3-2018-1077544'),
 (8950,35,'Global Trance Grooves 187 (09-10-2018) Ovnimoon Guestmix','2020-02-07 00:00:00',0,'https://39s-a.musify.club/img/71/20320543/52349733.jpg','https://musify.club/release/global-trance-grooves-187-09-10-2018-ovnimoon-guestmix-2018-1246402'),
 (8951,35,'Stapanii Timpului','2018-08-28 00:00:00',0,'https://38s-a.musify.club/img/70/16991189/45023196.jpg','https://musify.club/release/e-mantra-stapanii-timpului-2018-1036149'),
 (8952,35,'Big Workout &amp; Fitness Music Vol.2','2018-10-15 00:00:00',0,'https://39s-a.musify.club/img/70/17354857/45765527.jpg','https://musify.club/release/big-workout-and-fitness-music-vol-2-2018-1056511'),
 (8953,35,'Big Workout &amp; Fitness Music','2018-09-22 00:00:00',0,'https://40s-a.musify.club/img/70/17195441/45480903.jpg','https://musify.club/release/big-workout-and-fitness-music-2018-1047141'),
 (8954,35,'Spiritual Transformations (2018)','2019-10-25 00:00:00',0,'https://38s-a.musify.club/img/70/19850145/51200873.jpg','https://musify.club/release/spiritual-transformations-2018-2018-1217862'),
 (8955,35,'Serenity','2018-10-17 00:00:00',0,'https://40s-a.musify.club/img/70/17365468/45783164.jpg','https://musify.club/release/e-mantra-serenity-2018-1056760'),
 (8956,35,'Elan Vital','2018-11-10 00:00:00',0,'https://37s-a.musify.club/img/68/17530074/46188382.jpg','https://musify.club/release/elan-vital-2018-1067258'),
 (8957,35,'Psy Goa Trance. Beach Party','2019-02-18 00:00:00',0,'https://39s.musify.club/img/69/18178160/47732664.jpg','https://musify.club/release/psy-goa-trance-beach-party-2018-1107650'),
 (8958,35,'Future Psychedelic Trance','2019-02-10 00:00:00',0,'https://41s.musify.club/img/69/18119446/47657253.jpg','https://musify.club/release/future-psychedelic-trance-2018-1104548'),
 (8959,35,'Folding Time','2017-10-16 00:00:00',0,'https://41s-a.musify.club/img/71/14881989/37791269.jpg','https://musify.club/release/e-mantra-folding-time-2017-919222'),
 (8960,35,'Serial Chillers 5CD','2017-10-15 00:00:00',0,'https://41s-a.musify.club/img/71/14867446/37759987.jpg','https://musify.club/release/serial-chillers-5cd-2017-918738'),
 (8961,35,'Goa Trance Legacy 2','2017-01-18 00:00:00',0,'https://41s.musify.club/img/68/12835995/33242606.jpg','https://musify.club/release/goa-trance-legacy-2-2017-803922'),
 (8962,35,'604 Syndroms','2017-02-05 00:00:00',0,'https://41s.musify.club/img/68/12965114/33512380.jpg','https://musify.club/release/604-syndroms-2017-811076'),
 (8963,35,'Compendium Vol. I','2017-05-10 00:00:00',0,'https://41s.musify.club/img/69/13741426/35334660.jpg','https://musify.club/release/e-mantra-compendium-vol-i-2017-851512'),
 (8964,35,'Guardians Of Light','2017-09-24 00:00:00',0,'https://41s-a.musify.club/img/70/14704896/37420435.jpg','https://musify.club/release/guardians-of-light-2017-909791'),
 (8965,35,'Euro Truck Simulator 2 Vol.5','2017-01-23 00:00:00',0,'https://41s.musify.club/img/68/12870786/33323061.jpg','https://musify.club/release/euro-truck-simulator-2-vol-5-2017-806110'),
 (8966,35,'Magic Wind Psychedelic Trance Mix','2017-01-11 00:00:00',0,'https://41s.musify.club/img/68/12799943/33164837.jpg','https://musify.club/release/magic-wind-psychedelic-trance-mix-2017-802020'),
 (8967,35,'BGF Chronicles','2017-07-22 00:00:00',0,'https://41s.musify.club/img/69/14229718/36361198.jpg','https://musify.club/release/bgf-chronicles-2017-878985'),
 (8968,35,'Selection Of The Finest Psytrance Chapter 2','2017-09-22 00:00:00',0,'https://41s-a.musify.club/img/71/14663699/37360760.jpg','https://musify.club/release/selection-of-the-finest-psytrance-chapter-2-2017-908253'),
 (8969,35,'Psy Trance Treasures 2017 - 100 Best of Top Full-on Progressive &amp; Psychedelic Goa Hits','2020-12-06 00:00:00',0,'https://38s.musify.club/img/68/22701079/58177613.jpg','https://musify.club/release/psy-trance-treasures-2017-100-best-of-top-full-on-progressive-and-psyc-2017-1410113'),
 (8970,35,'Symphony GOA','2020-04-09 00:00:00',0,'https://39s.musify.club/img/69/20851003/53292920.jpg','https://musify.club/release/symphony-goa-2017-1273479'),
 (8971,35,'World Of Color Madness','2020-04-10 00:00:00',0,'https://39s-a.musify.club/img/71/20835607/53278701.jpg','https://musify.club/release/world-of-color-madness-2017-1273763'),
 (8972,35,'Progressive Goa Trance 2017 Top 100 Hits DJ Mix','2020-07-24 00:00:00',0,'https://39s-a.musify.club/img/70/21629812/54965522.jpg','https://musify.club/release/progressive-goa-trance-2017-top-100-hits-dj-mix-2016-1321968'),
 (8973,35,'Goa Moon Vol. 8','2016-07-22 00:00:00',0,'https://40s.musify.club/img/69/11770164/30968283.jpg','https://musify.club/release/goa-moon-vol-8-2016-745385'),
 (8974,35,'Oblivion','2016-12-05 00:00:00',0,'https://41s.musify.club/img/69/12571706/32689989.jpg','https://musify.club/release/e-mantra-oblivion-2016-789400'),
 (8975,35,'Floating Spirals III','2016-06-15 00:00:00',0,'https://39s-a.musify.club/img/70/11587699/30594797.jpg','https://musify.club/release/floating-spirals-iii-2016-734832'),
 (8976,35,'Psykokinesis Vol. 1','2016-08-16 00:00:00',0,'https://38s-a.musify.club/img/70/11911140/31263103.jpg','https://musify.club/release/psykokinesis-vol-1-2016-753193'),
 (8977,35,'Wind Of Buri - &#39;Chill Out Zone&#39; Five Years Anniversary Special Mix (Part 3)','2020-03-04 00:00:00',0,'https://38s-a.musify.club/img/70/20562650/52721426.jpg','https://musify.club/release/wind-of-buri-chill-out-zone-five-years-anniversary-special-mix-part-3-2016-1258557'),
 (8978,35,'Somnium','2016-08-10 00:00:00',0,'https://39s-a.musify.club/img/70/11884991/31200156.jpg','https://musify.club/release/e-mantra-somnium-2016-751503'),
 (8979,35,'Signals Re-Edit','2016-05-09 00:00:00',0,'https://38s-a.musify.club/img/70/11396883/30178134.jpg','https://musify.club/release/e-mantra-signals-re-edit-2016-724206'),
 (8980,35,'Ancestral Lullabies 2','2016-01-23 00:00:00',0,'https://40s.musify.club/img/69/10724702/28720074.jpg','https://musify.club/release/ancestral-lullabies-2-2016-681842'),
 (8981,35,'Aerial','2018-08-21 00:00:00',0,'https://38s.musify.club/img/69/16939204/44852414.jpg','https://musify.club/release/aerial-2016-1033556'),
 (8982,35,'Psy Trance Treasures 2017','2016-12-26 00:00:00',0,'https://41s.musify.club/img/69/12689673/32941309.jpg','https://musify.club/release/psy-trance-treasures-2017-2016-795717'),
 (8983,35,'Fata Morgana (Journeys in Goa Trance)','2016-02-24 00:00:00',0,'https://37s-a.musify.club/img/71/10950055/29232429.jpg','https://musify.club/release/fata-morgana-journeys-in-goa-trance-2016-697529'),
 (8984,35,'Ascending','2017-09-25 00:00:00',0,'https://41s-a.musify.club/img/71/14711782/37436732.jpg','https://musify.club/release/ascending-2016-910438'),
 (8985,35,'Flying Over The Universe','2016-06-06 00:00:00',0,'https://39s.musify.club/img/68/11528620/30472089.jpg','https://musify.club/release/flying-over-the-universe-2016-731926'),
 (8986,35,'Top 30 Progressive Tracks Vol. 2','2021-11-22 00:00:00',0,'https://38s.musify.club/img/68/24807014/62669762.jpg','https://musify.club/release/top-30-progressive-tracks-vol-2-2015-1556139'),
 (8987,35,'Kogaion','2015-10-11 00:00:00',0,'https://37s.musify.club/img/69/10156649/26476766.jpg','https://musify.club/release/kogaion-2015-653679'),
 (8988,35,'Ru-Bix &amp; Jester: Supermoon 2015','2015-04-24 00:00:00',0,'https://37s.musify.club/img/69/9347210/24789290.jpg','https://musify.club/release/ru-bix-and-jester-supermoon-2015-2015-615384'),
 (8989,35,'Wind Of Buri - Masterminds Of Miracles 067 - E-Mantra (Part 1)','2020-03-03 00:00:00',0,'https://39s-a.musify.club/img/70/20527551/52700362.jpg','https://musify.club/release/wind-of-buri-masterminds-of-miracles-067-e-mantra-part-1-2015-1257833'),
 (8990,35,'Locomotion','2015-12-12 00:00:00',0,'https://39s-a.musify.club/img/71/10486561/27128514.jpg','https://musify.club/release/locomotion-2015-669174'),
 (8991,35,'Fall (Compiled By Dj Zen)','2015-12-15 00:00:00',0,'https://39s-a.musify.club/img/71/10498704/27167171.jpg','https://musify.club/release/fall-compiled-by-dj-zen-2015-670244'),
 (8992,35,'Raining Lights','2016-01-07 00:00:00',0,'https://38s-a.musify.club/img/70/10599654/27579703.jpg','https://musify.club/release/e-mantra-raining-lights-2015-677366'),
 (8993,35,'The Mystery of Crystal Worlds','2016-01-07 00:00:00',0,'https://38s-a.musify.club/img/70/10612771/27601708.jpg','https://musify.club/release/the-mystery-of-crystal-worlds-2015-677354'),
 (8994,35,'Viziuni Nocturne','2015-11-05 00:00:00',0,'https://39s.musify.club/img/69/10269758/26699283.jpg','https://musify.club/release/viziuni-nocturne-2015-659459'),
 (8995,35,'Frequencies','2015-11-21 00:00:00',0,'https://40s-a.musify.club/img/70/10394945/26928824.jpg','https://musify.club/release/frequencies-2015-664377'),
 (8996,35,'Evolutive Perceptions','2015-11-18 00:00:00',0,'https://40s-a.musify.club/img/70/10368531/26894433.jpg','https://musify.club/release/evolutive-perceptions-2015-663883'),
 (8997,35,'Organic Beats Vol​.​ 4','2015-09-06 00:00:00',0,'https://39s.musify.club/img/68/10009294/26158957.jpg','https://musify.club/release/organic-beats-vol-4-2015-645930'),
 (8998,35,'Top 40 Ambient Tracks, Vol.2','2022-02-06 00:00:00',0,'https://41s-a.musify.club/img/71/24290570/63236381.jpg','https://musify.club/release/top-40-ambient-tracks-vol-2-2014-1576041'),
 (8999,35,'Mobile Phone Mp3 Ringtones','2013-04-05 00:00:00',0,'https://38s.musify.club/img/69/4807044/12791451.jpg','https://musify.club/release/mobile-phone-mp3-ringtones-2014-375149'),
 (9000,35,'Wind Of Buri - Moments Of Life 089 (Psy Chill Mix)','2015-04-26 00:00:00',0,'https://39s-a.musify.club/img/71/9350925/24835721.jpg','https://musify.club/release/wind-of-buri-moments-of-life-089-psy-chill-mix-2014-615755'),
 (9001,35,'Heart of Goa Vol. 3','2014-11-21 00:00:00',0,'https://39s.musify.club/img/68/8510144/22878855.jpg','https://musify.club/release/heart-of-goa-vol-3-2014-574559'),
 (9002,35,'Summer','2019-07-24 00:00:00',0,'https://40s-a.musify.club/img/71/19230333/49878346.jpg','https://musify.club/release/summer-2014-1177426'),
 (9003,35,'Floating Spirals 2','2014-06-02 00:00:00',0,'https://40s.musify.club/img/69/7402934/20830318.jpg','https://musify.club/release/floating-spirals-2-2014-508714'),
 (9004,35,'Nemesis','2014-01-31 00:00:00',0,'https://37s-a.musify.club/img/68/6557546/27811859.jpg','https://musify.club/release/e-mantra-nemesis-2014-468848'),
 (9005,35,'Ten Spins Around the Sun CD1','2014-05-07 00:00:00',0,'https://38s.musify.club/img/69/7238549/20475572.jpg','https://musify.club/release/ten-spins-around-the-sun-cd1-2014-500367'),
 (9006,35,'Ten Spins Around the Sun CD2: Remixes','2014-05-07 00:00:00',0,'https://38s.musify.club/img/69/7238900/20475622.jpg','https://musify.club/release/ten-spins-around-the-sun-cd2-remixes-2014-500368'),
 (9007,35,'Voyager: Second Plateau','2014-09-21 00:00:00',0,'https://37s-a.musify.club/img/71/8054192/22085651.jpg','https://musify.club/release/voyager-second-plateau-2014-547070'),
 (9008,35,'Protozoa Vol. III','2014-03-17 00:00:00',0,'https://38s-a.musify.club/img/70/6873531/19767175.jpg','https://musify.club/release/protozoa-vol-iii-2014-482499'),
 (9009,35,'Wind Of Buri-Moments Of Life 098 (Dark Ambient Mix)','2016-07-05 00:00:00',0,'https://40s.musify.club/img/69/11692607/30814358.jpg','https://musify.club/release/wind-of-buri-moments-of-life-098-dark-ambient-mix-2014-741583'),
 (9010,35,'Goa 100 Goa Trance Hits 2014','2020-09-12 00:00:00',0,'https://39s.musify.club/img/68/22014721/56548568.jpg','https://musify.club/release/goa-100-goa-trance-hits-2014-2014-1374500'),
 (9011,35,'Sakura','2014-03-04 00:00:00',0,'https://38s-a.musify.club/img/70/6796906/19610715.jpg','https://musify.club/release/sakura-2014-478401'),
 (9012,35,'Summer','2014-07-27 00:00:00',0,'https://37s-a.musify.club/img/71/7727928/21439100.jpg','https://musify.club/release/summer-2014-523782'),
 (9013,35,'Echoes from the Void','2014-12-01 00:00:00',0,'https://37s.musify.club/img/69/8566020/23008125.jpg','https://musify.club/release/echoes-from-the-void-2014-577187'),
 (9014,35,'Goa Psy Trance Hits Vol. 1','2014-08-08 00:00:00',0,'https://37s-a.musify.club/img/71/7783256/21554044.jpg','https://musify.club/release/goa-psy-trance-hits-vol-1-2014-526849'),
 (9015,35,'Wind Of Buri - Cities In The Clouds 25','2015-02-20 00:00:00',0,'https://37s.musify.club/img/69/8991248/24031819.jpg','https://musify.club/release/wind-of-buri-cities-in-the-clouds-25-2013-598808'),
 (9016,35,'Voyager: First Plateau','2013-02-19 00:00:00',0,'https://39s.musify.club/img/69/4672526/15407152.jpg','https://musify.club/release/voyager-first-plateau-2013-364136'),
 (9017,35,'The Hermit&#39;s Sanctuary','2013-04-22 00:00:00',0,'https://38s.musify.club/img/69/4989893/13188407.jpg','https://musify.club/release/e-mantra-the-hermits-sanctuary-2013-378375'),
 (9018,35,'Dense (3) ‎– Exhale','2016-01-30 00:00:00',0,'https://39s-a.musify.club/img/70/10749187/28863314.jpg','https://musify.club/release/dense-3-exhale-2013-686658'),
 (9019,35,'Trancemutation of the Mind','2017-08-29 00:00:00',0,'https://40s-a.musify.club/img/70/16971040/44904919.jpg','https://musify.club/release/trancemutation-of-the-mind-2013-898368'),
 (9020,35,'Sweet Passion Dream Vol. 5 - ZAA','2016-05-17 00:00:00',0,'https://40s.musify.club/img/69/11451797/30288039.jpg','https://musify.club/release/sweet-passion-dream-vol-5-zaa-2013-727450'),
 (9021,35,'Wind Of Buri-Moments Of Life 079 (Psy Chill Mix)','2015-09-09 00:00:00',0,'https://39s.musify.club/img/69/10021730/26177644.jpg','https://musify.club/release/wind-of-buri-moments-of-life-079-psy-chill-mix-2013-646369'),
 (9022,35,'Top 40 Ambient Tracks','2022-02-06 00:00:00',0,'https://37s-a.musify.club/img/71/24279777/63243096.jpg','https://musify.club/release/top-40-ambient-tracks-2013-1576291'),
 (9023,35,'Imminent Satori','2013-10-21 00:00:00',0,'https://39s-a.musify.club/img/70/5895340/17980145.jpg','https://musify.club/release/imminent-satori-2013-432530'),
 (9024,35,'Silence','2012-06-30 00:00:00',0,'https://37s-a.musify.club/img/68/3681951/16273635.jpg','https://musify.club/release/e-mantra-silence-2012-314186'),
 (9025,35,'Wind Of Buri - Cities In The Clouds 21','2015-10-24 00:00:00',0,'https://39s.musify.club/img/69/10215207/26617225.jpg','https://musify.club/release/wind-of-buri-cities-in-the-clouds-21-2012-656588'),
 (9026,35,'Full On Fluoro 015 (24.07.2012)','2012-08-09 00:00:00',0,'https://41s-a.musify.club/img/70/3810174/39626542.jpg','https://musify.club/release/full-on-fluoro-015-24-07-2012-2012-320954'),
 (9027,35,'Space Of Power: The Legend About Great Existence Of The Universe [CD 1]','2012-11-10 00:00:00',0,'https://39s.musify.club/img/69/4169294/15413523.jpg','https://musify.club/release/space-of-power-the-legend-about-great-existence-of-the-universe-cd-1-2012-342058'),
 (9028,35,'Organic Beats Vol​. ​2','2013-12-02 00:00:00',0,'https://39s-a.musify.club/img/71/6091404/28023265.jpg','https://musify.club/release/organic-beats-vol-2-2012-442459'),
 (9029,35,'Space Of Power: The Legend About Great Existence Of The Universe [CD 2]','2012-11-10 00:00:00',0,'https://39s.musify.club/img/69/4169297/15371247.jpg','https://musify.club/release/space-of-power-the-legend-about-great-existence-of-the-universe-cd-2-2012-342059'),
 (9030,35,'Psychology Vol. 3','2019-05-02 00:00:00',0,'https://40s-a.musify.club/img/71/18675893/48720556.jpg','https://musify.club/release/psychology-vol-3-2012-1139555'),
 (9031,35,'Wind Of Buri-Moments Of Life 052 (Psy Chill Mix)','2016-06-24 00:00:00',0,'https://40s.musify.club/img/69/11638886/30698081.jpg','https://musify.club/release/wind-of-buri-moments-of-life-052-psy-chill-mix-2012-738366'),
 (9032,35,'Wind Of Buri-Moments Of Life 036 (Atmospheric Breaks Mix)','2016-11-17 00:00:00',0,'https://41s.musify.club/img/69/12460848/32455636.jpg','https://musify.club/release/wind-of-buri-moments-of-life-036-atmospheric-breaks-mix-2012-783573'),
 (9033,35,'Wind Of Buri - Moments Of Life 031 (Psy Chill Mix)','2016-11-17 00:00:00',0,'https://41s.musify.club/img/69/12460642/32455478.jpg','https://musify.club/release/wind-of-buri-moments-of-life-031-psy-chill-mix-2012-783570'),
 (9034,35,'Floating Spirals','2012-11-26 00:00:00',0,'https://39s.musify.club/img/69/4232664/15449887.jpg','https://musify.club/release/floating-spirals-2012-345673'),
 (9035,35,'Atlantis','2014-03-29 00:00:00',0,'https://38s-a.musify.club/img/70/6940602/19922820.jpg','https://musify.club/release/atlantis-2012-486236'),
 (9036,35,'New Age Style - Greatest New Age Hits, Vol. 7','2016-11-30 00:00:00',0,'https://41s.musify.club/img/69/12532628/32622391.jpg','https://musify.club/release/new-age-style-greatest-new-age-hits-vol-7-2012-787757'),
 (9037,35,'Wind Of Buri - Cities In The Clouds 13','2017-01-05 00:00:00',0,'https://41s.musify.club/img/68/12753692/33087226.jpg','https://musify.club/release/wind-of-buri-cities-in-the-clouds-13-2012-799947'),
 (9038,35,'Wind Of Buri - Cities In The Clouds 09','2017-01-05 00:00:00',0,'https://41s.musify.club/img/68/12753511/33087139.jpg','https://musify.club/release/wind-of-buri-cities-in-the-clouds-09-2012-799945'),
 (9039,35,'Chillout Coast #05','2013-01-16 00:00:00',0,'https://39s.musify.club/img/69/4500893/15342105.jpg','https://musify.club/release/chillout-coast-05-2012-357027'),
 (9040,35,'Wind Of Buri-Moments Of Life 44 (Psy Chill Mix)','2016-11-17 00:00:00',0,'https://41s.musify.club/img/69/12461001/32455749.jpg','https://musify.club/release/wind-of-buri-moments-of-life-44-psy-chill-mix-2012-783575'),
 (9041,35,'Global Trance Grooves 097 (10-05-2011) Eleusyn Guestmix','2020-01-19 00:00:00',0,'https://38s.musify.club/img/69/20297167/52120881.jpg','https://musify.club/release/global-trance-grooves-097-10-05-2011-eleusyn-guestmix-2011-1240742'),
 (9042,35,'Astrograms','2011-05-21 00:00:00',0,'https://41s-a.musify.club/img/70/1160568/40229248.jpg','https://musify.club/release/astrograms-2011-214805'),
 (9043,35,'Visions From The Past','2011-05-20 00:00:00',0,'https://39s.musify.club/img/68/8751414/23350572.jpg','https://musify.club/release/e-mantra-visions-from-the-past-2011-214577'),
 (9044,35,'Erta Al&#233;','2016-01-31 00:00:00',0,'https://40s.musify.club/img/69/10766712/28882990.jpg','https://musify.club/release/erta-ale-2011-687387'),
 (9045,35,'Planet Riders','2011-09-13 00:00:00',0,'https://41s-a.musify.club/img/70/1248996/40139226.jpg','https://musify.club/release/planet-riders-2011-244258'),
 (9046,35,'Pathfinder','2011-12-07 00:00:00',0,'https://39s.musify.club/img/69/1830397/15504404.jpg','https://musify.club/release/e-mantra-pathfinder-2011-264231'),
 (9047,35,'Ether','2013-12-02 00:00:00',0,'https://39s-a.musify.club/img/71/6091272/28023656.jpg','https://musify.club/release/ether-2010-442454'),
 (9048,35,'Spiritual Rising','2013-04-05 00:00:00',0,'https://38s.musify.club/img/69/4763573/12789307.jpg','https://musify.club/release/spiritual-rising-2010-375023'),
 (9049,35,'Kumharas 7 - Ibiza Sunset Ambient','2020-09-13 00:00:00',0,'https://40s.musify.club/img/68/22049366/57321373.jpg','https://musify.club/release/kumharas-7-ibiza-sunset-ambient-2010-1374662'),
 (9050,35,'Goa Moon, vol. 2.2','2019-06-18 00:00:00',0,'https://40s-a.musify.club/img/71/18999220/49378445.jpg','https://musify.club/release/goa-moon-vol-2-2-2009-1159413'),
 (9051,35,'Arcana','2013-10-05 00:00:00',0,'https://39s-a.musify.club/img/70/5577958/17517130.jpg','https://musify.club/release/e-mantra-arcana-2009-429785'),
 (9052,35,'Organic Vision Vol.1','2015-11-28 00:00:00',0,'https://40s-a.musify.club/img/70/10368451/26992273.jpg','https://musify.club/release/organic-vision-vol-1-2009-665689'),
 (9053,35,'Opus Iridium [CD 1]','2015-12-05 00:00:00',0,'https://40s-a.musify.club/img/70/10396793/27060891.jpg','https://musify.club/release/opus-iridium-cd-1-2008-667261'),
 (9054,35,'Opus Iridium [CD 2]','2015-12-05 00:00:00',0,'https://40s-a.musify.club/img/70/10396810/27060897.jpg','https://musify.club/release/opus-iridium-cd-2-2008-667264'),
 (9055,35,'Signals','2014-05-07 00:00:00',0,'https://38s.musify.club/img/69/7235488/20469227.jpg','https://musify.club/release/e-mantra-signals-2008-500190'),
 (9056,35,'Mainspring Motion','2018-09-12 00:00:00',0,'https://40s-a.musify.club/img/70/17105766/45374170.jpg','https://musify.club/release/mainspring-motion-2007-1043346'),
 (9057,35,'Goa Vol. 18 (CD1)','2016-06-15 00:00:00',0,'https://38s-a.musify.club/img/70/11582265/30587128.jpg','https://musify.club/release/goa-vol-18-cd1-2006-735078'),
 (9058,35,'Bombay Beats (CD1)','2013-05-26 00:00:00',0,'https://40s-a.musify.club/img/70/5050865/13894886.jpg','https://musify.club/release/bombay-beats-cd1-2006-386640'),
 (9059,35,'Trancelaciya Vol 04 (Goa Edition) [CD2] - New School','2016-02-27 00:00:00',0,'https://38s-a.musify.club/img/70/10964173/29271496.jpg','https://musify.club/release/trancelaciya-vol-04-goa-edition-cd2-new-school-698767'),
 (9060,34,'New Music Releases Week 23 Of 2022 #3','2022-06-19 00:00:00',0,'https://40s-a.musify.club/img/70/25936553/64918379.jpg','https://musify.club/release/new-music-releases-week-23-of-2022-3-2022-1630547'),
 (9061,34,'The Space Furies: Synthwave Mix','2022-06-10 00:00:00',0,'https://39s.musify.club/img/68/25902423/64856667.jpg','https://musify.club/release/the-space-furies-synthwave-mix-2022-1628783'),
 (9062,34,'MATRIX','2021-08-10 00:00:00',0,'https://38s.musify.club/img/68/24436851/61813299.jpg','https://musify.club/release/boris-brejcha-matrix-2021-1532204'),
 (9063,34,'New Music Releases Week 23 Vol.1','2021-06-18 00:00:00',0,'https://39s-a.musify.club/img/71/24113880/61152868.jpg','https://musify.club/release/new-music-releases-week-23-vol-1-2021-1509532'),
 (9064,34,'New Music Releases Week 27 Of 2021 PT.3','2021-10-02 00:00:00',0,'https://37s-a.musify.club/img/71/24272834/62293911.jpg','https://musify.club/release/new-music-releases-week-27-of-2021-pt-3-2021-1544932'),
 (9065,34,'EXIT','2021-06-11 00:00:00',0,'https://38s.musify.club/img/68/24078603/61075416.jpg','https://musify.club/release/boris-brejcha-exit-2021-1507126'),
 (9066,34,'Never Stop Dancing','2022-01-19 00:00:00',0,'https://41s.musify.club/img/69/24930801/62990546.jpg','https://musify.club/release/never-stop-dancing-2021-1566927'),
 (9067,34,'Collection Of House Styles 8 [Compiled By Tokarilo] PART2','2021-05-18 00:00:00',0,'https://38s.musify.club/img/68/23878990/60677360.jpg','https://musify.club/release/collection-of-house-styles-8-compiled-by-tokarilo-part2-2021-1494690'),
 (9068,34,'Mayday 2020 Past: Present: Future (The Official Mayday Compilation 2020)','2020-04-15 00:00:00',0,'https://39s.musify.club/img/69/20876001/53369953.jpg','https://musify.club/release/mayday-2020-past-present-future-the-official-mayday-compilation-2020-2020-1276403'),
 (9069,34,'Uplifting Trance KM-4','2020-06-09 00:00:00',0,'https://39s-a.musify.club/img/71/21298909/54278358.jpg','https://musify.club/release/uplifting-trance-km-4-2020-1300117'),
 (9070,34,'Space Diver','2020-01-28 00:00:00',0,'https://38s-a.musify.club/img/70/20325589/52188890.jpg','https://musify.club/release/boris-brejcha-space-diver-2020-1241826'),
 (9071,34,'Serious Beats 95 CD 2','2020-11-06 00:00:00',0,'https://38s-a.musify.club/img/71/22473295/57752245.jpg','https://musify.club/release/serious-beats-95-cd-2-2020-1397047'),
 (9072,34,'Melodic Techno','2020-05-16 00:00:00',0,'https://39s-a.musify.club/img/71/21119079/53877815.jpg','https://musify.club/release/melodic-techno-2020-1290581'),
 (9073,34,'Dance Club Vol.200 CD2','2020-10-12 00:00:00',0,'https://38s.musify.club/img/68/22286457/57554275.jpg','https://musify.club/release/dance-club-vol-200-cd2-2020-1386942'),
 (9074,34,'Club Sound (Vol.10)','2019-12-19 00:00:00',0,'https://39s-a.musify.club/img/70/20158290/51841019.jpg','https://musify.club/release/club-sound-vol-10-2019-1234723'),
 (9075,34,'The Works Of Boris Brejcha','2019-11-08 00:00:00',0,'https://39s-a.musify.club/img/70/19934613/51391525.jpg','https://musify.club/release/the-works-of-boris-brejcha-2019-1222900'),
 (9076,34,'Butterflies','2019-09-07 00:00:00',0,'https://40s-a.musify.club/img/71/19509654/50483117.jpg','https://musify.club/release/boris-brejcha-butterflies-2019-1195060'),
 (9077,34,'I Love Techno Music','2019-08-30 00:00:00',0,'https://40s-a.musify.club/img/71/19416389/50332897.jpg','https://musify.club/release/i-love-techno-music-2019-1188784'),
 (9078,34,'Electro, Deep, Techno House &amp; Trance Vol. 16 (2018)','2019-10-27 00:00:00',0,'https://38s.musify.club/img/69/19860609/51228877.jpg','https://musify.club/release/electro-deep-techno-house-and-trance-vol-16-2018-2018-1218377'),
 (9079,34,'Solarstone Presents Pure Trance Radio 132 (04-04-2018)','2018-06-30 00:00:00',0,'https://38s-a.musify.club/img/70/16588853/44095165.jpg','https://musify.club/release/solarstone-presents-pure-trance-radio-132-04-04-2018-2018-1012468'),
 (9080,34,'Angular Momentum (CD1)','2018-09-16 00:00:00',0,'https://41s-a.musify.club/img/70/17136438/45404949.jpg','https://musify.club/release/angular-momentum-cd1-2018-1044741'),
 (9081,34,'Devil','2018-07-24 00:00:00',0,'https://37s.musify.club/img/69/16760499/44453297.jpg','https://musify.club/release/boris-brejcha-devil-2018-1022664'),
 (9082,34,'Selection Of The Finest Psytrance Chapter 3','2017-09-19 00:00:00',0,'https://41s-a.musify.club/img/71/14663703/37330171.jpg','https://musify.club/release/selection-of-the-finest-psytrance-chapter-3-2017-907715'),
 (9083,34,'Space Gremlin (Unreleased)','2017-07-12 00:00:00',0,'https://41s.musify.club/img/68/14150132/36196832.jpg','https://musify.club/release/boris-brejcha-space-gremlin-unreleased-2017-874106'),
 (9084,34,'Selection Of The Finest Psytrance Chapter 1','2017-09-19 00:00:00',0,'https://41s-a.musify.club/img/71/14663693/37330165.jpg','https://musify.club/release/selection-of-the-finest-psytrance-chapter-1-2017-907714'),
 (9085,34,'Melodic Techno Tom II','2017-07-26 00:00:00',0,'https://41s.musify.club/img/69/14253724/36414060.jpg','https://musify.club/release/melodic-techno-tom-ii-2017-880435'),
 (9086,34,'Discoteka 2017 Dance Club Vol. 159','2016-12-31 00:00:00',0,'https://41s.musify.club/img/69/12731914/33017504.jpg','https://musify.club/release/discoteka-2017-dance-club-vol-159-2016-797880'),
 (9087,34,'Techno Vol. 2','2017-01-05 00:00:00',0,'https://41s.musify.club/img/69/12764513/33082532.jpg','https://musify.club/release/techno-vol-2-2016-799693'),
 (9088,34,'Impulse2 CD1','2020-06-05 00:00:00',0,'https://38s.musify.club/img/68/21283654/54230138.jpg','https://musify.club/release/impulse2-cd1-2016-1298854'),
 (9089,34,'FEAR','2016-11-06 00:00:00',0,'https://41s.musify.club/img/69/12402101/32328015.jpg','https://musify.club/release/boris-brejcha-fear-2016-779985'),
 (9090,34,'TOP 100 Deep House November','2016-12-08 00:00:00',0,'https://41s.musify.club/img/69/12580147/32719243.jpg','https://musify.club/release/top-100-deep-house-november-2016-790006'),
 (9091,34,'Neotrance Vol.4','2017-05-07 00:00:00',0,'https://41s.musify.club/img/68/13725597/35307608.jpg','https://musify.club/release/neotrance-vol-4-2016-851151'),
 (9092,34,'Fckng Serious One Year','2016-11-19 00:00:00',0,'https://41s.musify.club/img/68/12471810/32471774.jpg','https://musify.club/release/fckng-serious-one-year-2016-783837'),
 (9093,34,'DJ Mixes Single Tracks','2016-11-30 00:00:00',0,'https://41s.musify.club/img/68/12541689/32612430.jpg','https://musify.club/release/boris-brejcha-dj-mixes-single-tracks-2016-787502'),
 (9094,34,'22','2016-02-23 00:00:00',0,'https://38s-a.musify.club/img/70/10941274/29212042.jpg','https://musify.club/release/boris-brejcha-22-2016-696930'),
 (9095,34,'Electro, Deep, Techno House &amp; Trance Collection Vol. 5','2020-06-15 00:00:00',0,'https://40s-a.musify.club/img/71/21345908/54392423.jpg','https://musify.club/release/electro-deep-techno-house-and-trance-collection-vol-5-2016-1303576'),
 (9096,34,'Dance Club Vol. 149 CD1','2020-06-09 00:00:00',0,'https://39s.musify.club/img/68/21314135/54296228.jpg','https://musify.club/release/dance-club-vol-149-cd1-2016-1301019'),
 (9097,34,'Schleierwolken','2018-05-24 00:00:00',0,'https://37s-a.musify.club/img/71/16310070/43489016.jpg','https://musify.club/release/boris-brejcha-schleierwolken-2015-998431'),
 (9098,34,'Best Of Harthouse Digital Vol. 2','2015-12-17 00:00:00',0,'https://37s.musify.club/img/69/10505595/27188942.jpg','https://musify.club/release/best-of-harthouse-digital-vol-2-2015-670819'),
 (9099,34,'SAW','2018-05-24 00:00:00',0,'https://37s-a.musify.club/img/71/16310107/43489048.jpg','https://musify.club/release/saw-2015-998445'),
 (9100,34,'S.P.A.C.E.','2018-05-24 00:00:00',0,'https://37s-a.musify.club/img/71/16310010/43488976.jpg','https://musify.club/release/boris-brejcha-s-p-a-c-e-2015-998424'),
 (9101,34,'Vocal Tech House Vol.1','2015-11-09 00:00:00',0,'https://38s.musify.club/img/69/10329275/26816521.jpg','https://musify.club/release/vocal-tech-house-vol-1-2015-662408'),
 (9102,34,'Car Audio1','2015-04-06 00:00:00',0,'https://39s-a.musify.club/img/71/9255549/24557322.jpg','https://musify.club/release/car-audio1-2015-611314'),
 (9103,34,'Beatport Top 100 30.08.2014 - Minimal','2015-07-13 00:00:00',0,'https://39s.musify.club/img/68/9776250/25618673.jpg','https://musify.club/release/beatport-top-100-30-08-2014-minimal-2014-633444'),
 (9104,34,'Hashtag','2014-10-15 00:00:00',0,'https://37s.musify.club/img/69/8182927/22367229.jpg','https://musify.club/release/boris-brejcha-hashtag-2014-552941'),
 (9105,34,'Harthouse #Beatportdecade Techno','2014-12-06 00:00:00',0,'https://37s.musify.club/img/69/8620259/23075681.jpg','https://musify.club/release/harthouse-beatportdecade-techno-2014-579485'),
 (9106,34,'Feuerfalter, Part 02','2014-07-16 00:00:00',0,'https://37s-a.musify.club/img/71/7668695/21318692.jpg','https://musify.club/release/boris-brejcha-feuerfalter-part-02-2014-520577'),
 (9107,34,'Everybody Wants To Go To Heaven','2018-05-24 00:00:00',0,'https://37s-a.musify.club/img/71/16309968/43488953.jpg','https://musify.club/release/boris-brejcha-everybody-wants-to-go-to-heaven-2013-998457'),
 (9108,34,'Feuerfalter Part 01','2013-12-14 00:00:00',0,'https://39s-a.musify.club/img/71/6210169/27847642.jpg','https://musify.club/release/boris-brejcha-feuerfalter-part-01-2013-445562'),
 (9109,34,'Wego','2013-10-11 00:00:00',0,'https://39s-a.musify.club/img/70/5732904/17606135.jpg','https://musify.club/release/boris-brejcha-wego-2013-430622'),
 (9110,34,'Der Alchemyst','2013-10-21 00:00:00',0,'https://39s-a.musify.club/img/70/5895545/17980206.jpg','https://musify.club/release/boris-brejcha-der-alchemyst-2013-432535'),
 (9111,34,'Remixes','2016-06-20 00:00:00',0,'https://39s-a.musify.club/img/70/11616327/30646374.jpg','https://musify.club/release/remixes-2013-736854'),
 (9112,34,'Deep+ Vol.1','2013-12-31 00:00:00',0,'https://39s-a.musify.club/img/70/6376406/18812712.jpg','https://musify.club/release/deep-vol-1-2012-460357'),
 (9113,34,'Farbenfrohe Stadt','2018-05-24 00:00:00',0,'https://37s-a.musify.club/img/71/16309853/43488861.jpg','https://musify.club/release/boris-brejcha-farbenfrohe-stadt-2012-998422'),
 (9114,34,'Der Mensch Wird Zur Maschine','2018-05-24 00:00:00',0,'https://37s-a.musify.club/img/71/16309788/43488792.jpg','https://musify.club/release/boris-brejcha-der-mensch-wird-zur-maschine-2012-998438'),
 (9115,34,'Schaltzentrale - The Remixes','2018-05-24 00:00:00',0,'https://37s-a.musify.club/img/71/16309901/43488891.jpg','https://musify.club/release/boris-brejcha-schaltzentrale-the-remixes-2012-998423'),
 (9116,34,'That&#39;s The Funky Shit','2018-05-24 00:00:00',0,'https://37s-a.musify.club/img/71/16309940/43488926.jpg','https://musify.club/release/boris-brejcha-thats-the-funky-shit-2012-998448'),
 (9117,34,'The Best Remixes [V.I.P.] [Vol.1]','2012-03-30 00:00:00',0,'https://41s.musify.club/img/68/3380460/41355068.jpg','https://musify.club/release/the-best-remixes-v-i-p-vol-1-2012-294500'),
 (9118,34,'New Club Hits 2012 (Летний Сборник) 2','2012-06-05 00:00:00',0,'https://41s.musify.club/img/68/3611248/41243827.jpg','https://musify.club/release/new-club-hits-2012-letnii-sbornik-2-2012-309626'),
 (9119,34,'Sugar Baby','2018-05-24 00:00:00',0,'https://37s-a.musify.club/img/71/16309751/43488750.jpg','https://musify.club/release/boris-brejcha-sugar-baby-2011-998435'),
 (9120,34,'James Bond','2018-05-24 00:00:00',0,'https://37s-a.musify.club/img/71/16309682/43488697.jpg','https://musify.club/release/boris-brejcha-james-bond-2011-998419'),
 (9121,34,'R&#252;hrsch&#252;ssel','2018-05-24 00:00:00',0,'https://37s-a.musify.club/img/71/16309699/43488718.jpg','https://musify.club/release/boris-brejcha-ruhrschussel-2011-998420'),
 (9122,34,'My Name Is... Remixes','2011-06-20 00:00:00',0,'https://41s-a.musify.club/img/70/1186854/40202531.jpg','https://musify.club/release/boris-brejcha-my-name-is-remixes-2011-223541'),
 (9123,34,'Diffusor','2018-05-24 00:00:00',0,'https://37s-a.musify.club/img/71/16309655/43488664.jpg','https://musify.club/release/boris-brejcha-diffusor-2010-998418'),
 (9124,34,'My Name Is','2011-01-16 00:00:00',0,'https://41s-a.musify.club/img/70/1084907/40305673.jpg','https://musify.club/release/boris-brejcha-my-name-is-2010-114816'),
 (9125,34,'Magic Gum','2018-05-24 00:00:00',0,'https://37s-a.musify.club/img/71/16309637/43488655.jpg','https://musify.club/release/boris-brejcha-magic-gum-2009-998425'),
 (9126,34,'Joystick','2013-11-19 00:00:00',0,'https://39s-a.musify.club/img/70/6010645/18220477.jpg','https://musify.club/release/boris-brejcha-joystick-2009-439487'),
 (9127,34,'Commander Tom','2013-11-19 00:00:00',0,'https://41s.musify.club/img/68/6010629/41179914.jpg','https://musify.club/release/boris-brejcha-commander-tom-2009-439486'),
 (9128,34,'Schaltzentrale','2013-11-19 00:00:00',0,'https://39s-a.musify.club/img/70/6010615/18220455.jpg','https://musify.club/release/boris-brejcha-schaltzentrale-2009-439485'),
 (9129,34,'Global Clubbing Germany','2010-12-14 00:00:00',0,'https://41s-a.musify.club/img/70/1133328/40257246.jpg','https://musify.club/release/global-clubbing-germany-2009-205732'),
 (9130,34,'Mirror Of The Future','2018-05-24 00:00:00',0,'https://39s-a.musify.club/img/70/16309225/43488344.jpg','https://musify.club/release/mirror-of-the-future-2008-998405'),
 (9131,34,'Aquilah','2018-05-24 00:00:00',0,'https://37s-a.musify.club/img/71/16309563/43488593.jpg','https://musify.club/release/boris-brejcha-aquilah-2008-998421'),
 (9132,34,'Lost Memory','2018-05-24 00:00:00',0,'https://37s-a.musify.club/img/71/16309602/43488633.jpg','https://musify.club/release/boris-brejcha-lost-memory-2008-998417'),
 (9133,34,'Fruhe Autisten Revisited','2017-05-10 00:00:00',0,'https://41s.musify.club/img/69/13729940/35334960.jpg','https://musify.club/release/boris-brejcha-fruhe-autisten-revisited-2008-851603'),
 (9134,34,'Mein Wahres Ich','2012-03-19 00:00:00',0,'https://41s.musify.club/img/68/3345184/41373205.jpg','https://musify.club/release/boris-brejcha-mein-wahres-ich-2008-292096'),
 (9135,34,'Die Maschinen Kontrollieren Uns','2012-03-19 00:00:00',0,'https://41s.musify.club/img/68/3343760/41373823.jpg','https://musify.club/release/boris-brejcha-die-maschinen-kontrollieren-uns-2007-291994'),
 (9136,34,'White Snake','2018-05-24 00:00:00',0,'https://41s-a.musify.club/img/70/16309513/43489304.jpg','https://musify.club/release/boris-brejcha-white-snake-2007-998463'),
 (9137,34,'Outer Space','2018-05-24 00:00:00',0,'https://41s-a.musify.club/img/70/16309473/43489293.jpg','https://musify.club/release/boris-brejcha-outer-space-2007-998464'),
 (9138,34,'Fireworker (Remixes)','2018-05-24 00:00:00',0,'https://41s-a.musify.club/img/70/16309413/43489284.jpg','https://musify.club/release/boris-brejcha-fireworker-remixes-2007-998465'),
 (9139,34,'Monster In The Box (Remixes)','2018-05-24 00:00:00',0,'https://37s-a.musify.club/img/71/16309439/43488529.jpg','https://musify.club/release/boris-brejcha-monster-in-the-box-remixes-2007-998426'),
 (9140,34,'Die Milchstrasse','2018-05-24 00:00:00',0,'https://39s-a.musify.club/img/70/16309384/43488475.jpg','https://musify.club/release/boris-brejcha-die-milchstrasse-2007-998409'),
 (9141,34,'Who Is Your Man','2018-05-24 00:00:00',0,'https://37s-a.musify.club/img/71/16309557/43488584.jpg','https://musify.club/release/boris-brejcha-who-is-your-man-2007-998430'),
 (9142,34,'Die Maschinen Sind Gestrandet','2017-05-10 00:00:00',0,'https://41s.musify.club/img/69/13729780/35334930.jpg','https://musify.club/release/boris-brejcha-die-maschinen-sind-gestrandet-2007-851609'),
 (9143,34,'Yellow Kitchen','2018-05-24 00:00:00',0,'https://39s-a.musify.club/img/70/16309338/43488456.jpg','https://musify.club/release/boris-brejcha-yellow-kitchen-2006-998407'),
 (9144,34,'Monster','2018-05-24 00:00:00',0,'https://39s-a.musify.club/img/70/16309300/43488424.jpg','https://musify.club/release/boris-brejcha-monster-2006-998410'),
 (9145,34,'Gorod Traits (Mixed By Mario) [CD2]','2018-06-30 00:00:00',0,'https://38s-a.musify.club/img/70/16554946/44067185.jpg','https://musify.club/release/gorod-traits-mixed-by-mario-cd2-2006-1012731'),
 (9146,34,'Unreleased Tracks &amp; From Compilations','2013-09-22 00:00:00',0,'https://39s-a.musify.club/img/70/5239133/27851136.jpg','https://musify.club/release/unreleased-tracks-and-from-compilations-415032'),
 (9147,33,'Music for Traveling Minds','2021-04-07 00:00:00',0,'https://39s.musify.club/img/68/23597854/60039217.jpg','https://musify.club/release/talamasca-music-for-traveling-minds-2021-1473201'),
 (9148,33,'The Sound of Psy-Trance, vol. 13','2021-06-19 00:00:00',0,'https://38s-a.musify.club/img/71/24117307/61163023.jpg','https://musify.club/release/the-sound-of-psy-trance-vol-13-2021-1509743'),
 (9149,33,'Psychedelic DNA, vol. 2','2021-06-17 00:00:00',0,'https://38s.musify.club/img/68/24114054/61147057.jpg','https://musify.club/release/psychedelic-dna-vol-2-2021-1509290'),
 (9150,33,'Best of Dacru Records','2021-01-07 00:00:00',0,'https://38s-a.musify.club/img/71/22936842/58667067.jpg','https://musify.club/release/best-of-dacru-records-2021-1424868'),
 (9151,33,'Uncharted, vol. 18','2021-01-18 00:00:00',0,'https://38s.musify.club/img/68/23009505/58821388.jpg','https://musify.club/release/uncharted-vol-18-2021-1431229'),
 (9152,33,'The Sound of Psy-Trance, vol. 10','2021-04-15 00:00:00',0,'https://38s.musify.club/img/68/23661555/60191698.jpg','https://musify.club/release/the-sound-of-psy-trance-vol-10-2021-1478970'),
 (9153,33,'The Works Of Indra','2020-03-17 00:00:00',0,'https://38s.musify.club/img/69/20643019/52912070.jpg','https://musify.club/release/the-works-of-indra-2020-1263437'),
 (9154,33,'Beatport Psy Trance. Electro Sound Pack #162','2020-09-26 00:00:00',0,'https://38s-a.musify.club/img/71/22187547/57442149.jpg','https://musify.club/release/beatport-psy-trance-electro-sound-pack-162-2020-1380460'),
 (9155,33,'Old School for Raver, vol. 3','2020-10-25 00:00:00',0,'https://38s.musify.club/img/68/22408303/57681176.jpg','https://musify.club/release/talamasca-old-school-for-raver-vol-3-2020-1393330'),
 (9156,33,'Old School for Raver, vol. 2','2020-10-04 00:00:00',0,'https://38s-a.musify.club/img/71/22245240/57510063.jpg','https://musify.club/release/talamasca-old-school-for-raver-vol-2-2020-1383651'),
 (9157,33,'Old School for Raver, vol. 1','2020-09-28 00:00:00',0,'https://38s.musify.club/img/68/22201004/57459041.jpg','https://musify.club/release/talamasca-old-school-for-raver-vol-1-2020-1381034'),
 (9158,33,'Uncharted, vol. 16','2020-07-05 00:00:00',0,'https://39s-a.musify.club/img/71/21534174/54700016.jpg','https://musify.club/release/uncharted-vol-16-2020-1311980'),
 (9159,33,'Sensory Depravation Remixes','2020-10-05 00:00:00',0,'https://38s.musify.club/img/68/22245488/57510367.jpg','https://musify.club/release/talamasca-sensory-depravation-remixes-2020-1383750'),
 (9160,33,'The Renounced','2020-10-05 00:00:00',0,'https://38s.musify.club/img/68/22245533/57510433.jpg','https://musify.club/release/the-renounced-2020-1383757'),
 (9161,33,'Dubstep Rave Club Top 100 Best Selling Chart Hits','2020-07-23 00:00:00',0,'https://38s.musify.club/img/69/21645401/54950767.jpg','https://musify.club/release/dubstep-rave-club-top-100-best-selling-chart-hits-2020-1321594'),
 (9162,33,'Dubstep Party Top 100 Best Selling Chart Hits','2020-07-24 00:00:00',0,'https://40s-a.musify.club/img/71/21646371/54971710.jpg','https://musify.club/release/dubstep-party-top-100-best-selling-chart-hits-2020-1322098'),
 (9163,33,'Goa, vol. 72','2020-06-15 00:00:00',0,'https://39s.musify.club/img/68/21367511/54386848.jpg','https://musify.club/release/goa-vol-72-2020-1303419'),
 (9164,33,'The Experiment','2020-06-16 00:00:00',0,'https://40s-a.musify.club/img/71/21392070/54410347.jpg','https://musify.club/release/talamasca-the-experiment-2020-1304185'),
 (9165,33,'Rave Dance Party Top 100 Best Selling Chart Hits','2020-09-10 00:00:00',0,'https://39s.musify.club/img/68/22024002/56557933.jpg','https://musify.club/release/rave-dance-party-top-100-best-selling-chart-hits-2020-1373612'),
 (9166,33,'Beatport Trance. Electro Sound Pack #26','2020-03-20 00:00:00',0,'https://40s-a.musify.club/img/71/20671526/52949881.jpg','https://musify.club/release/beatport-trance-electro-sound-pack-26-2020-1264449'),
 (9167,33,'Acid House Rave Top 100 Best Selling Chart Hits','2020-09-10 00:00:00',0,'https://39s.musify.club/img/68/22023914/56557803.jpg','https://musify.club/release/acid-house-rave-top-100-best-selling-chart-hits-2020-1373610'),
 (9168,33,'Dat Archives Vol.1','2020-02-12 00:00:00',0,'https://39s.musify.club/img/69/20431738/52457655.jpg','https://musify.club/release/dat-archives-vol-1-2020-1251146'),
 (9169,33,'Goa 2020, vol. 1','2020-02-09 00:00:00',0,'https://40s-a.musify.club/img/71/20369878/52395472.jpg','https://musify.club/release/goa-2020-vol-1-2020-1248746'),
 (9170,33,'Somatic Cell. Hyperactive Psy Trance (2019)','2019-10-25 00:00:00',0,'https://38s.musify.club/img/69/19851015/51201873.jpg','https://musify.club/release/somatic-cell-hyperactive-psy-trance-2019-2019-1217889'),
 (9171,33,'Psy Trance Psychedelic (2019)','2019-10-29 00:00:00',0,'https://38s-a.musify.club/img/70/19886910/51266264.jpg','https://musify.club/release/psy-trance-psychedelic-2019-2019-1219427'),
 (9172,33,'From India To Israel','2019-07-05 00:00:00',0,'https://40s-a.musify.club/img/71/19096444/49590425.jpg','https://musify.club/release/from-india-to-israel-2019-1166512'),
 (9173,33,'The Works Of Deedrah','2019-12-11 00:00:00',0,'https://38s-a.musify.club/img/70/20114980/51763622.jpg','https://musify.club/release/the-works-of-deedrah-2019-1232659'),
 (9174,33,'Devilize','2019-08-30 00:00:00',0,'https://40s-a.musify.club/img/71/19411895/50322339.jpg','https://musify.club/release/devilize-2019-1188467'),
 (9175,33,'Club Sound Vol. 6','2019-11-22 00:00:00',0,'https://39s-a.musify.club/img/70/20039112/51591274.jpg','https://musify.club/release/club-sound-vol-6-2019-1228997'),
 (9176,33,'Mixtape Session #1','2019-06-08 00:00:00',0,'https://40s-a.musify.club/img/71/18923298/49230729.jpg','https://musify.club/release/mixtape-session-1-2019-1154424'),
 (9177,33,'Goa Trance 2010','2019-02-02 00:00:00',0,'https://40s-a.musify.club/img/70/18054279/47335448.jpg','https://musify.club/release/goa-trance-2010-2019-1099835'),
 (9178,33,'Exlusive Tracks Vol.1','2020-03-20 00:00:00',0,'https://39s.musify.club/img/69/20669321/52947673.jpg','https://musify.club/release/exlusive-tracks-vol-1-2018-1264379'),
 (9179,33,'Progressive Psychodelic Trance (Exlusive Tracks)','2019-02-18 00:00:00',0,'https://39s.musify.club/img/69/18177742/47732179.jpg','https://musify.club/release/progressive-psychodelic-trance-exlusive-tracks-2018-1107587'),
 (9180,33,'Psyworld (2018)','2019-10-07 00:00:00',0,'https://40s-a.musify.club/img/71/19715864/50909865.jpg','https://musify.club/release/psyworld-2018-2018-1209431'),
 (9181,33,'After Winter Comes Spring','2018-03-21 00:00:00',0,'https://41s-a.musify.club/img/71/15896299/42567627.jpg','https://musify.club/release/after-winter-comes-spring-2018-973430'),
 (9182,33,'We Gonna Rock the World!','2018-06-27 00:00:00',0,'https://39s-a.musify.club/img/70/16549300/44011616.jpg','https://musify.club/release/we-gonna-rock-the-world-2018-1011712'),
 (9183,33,'Psychedelic Brainworkers, vol. 1','2018-06-24 00:00:00',0,'https://37s-a.musify.club/img/68/16541572/43947665.jpg','https://musify.club/release/psychedelic-brainworkers-vol-1-2018-1011314'),
 (9184,33,'Клубная Вечеринка Vol.2 CD2','2019-10-29 00:00:00',0,'https://40s-a.musify.club/img/71/19883956/51260681.jpg','https://musify.club/release/klubnaya-vecherinka-vol-2-cd2-2018-1219311'),
 (9185,33,'Клубная Вечеринка CD2 ','2019-10-29 00:00:00',0,'https://38s-a.musify.club/img/70/19885457/51264235.jpg','https://musify.club/release/klubnaya-vecherinka-cd2-2018-1219370'),
 (9186,33,'Uncharted Vol. 8','2018-01-25 00:00:00',0,'https://38s.musify.club/img/69/15581236/39305730.jpg','https://musify.club/release/uncharted-vol-8-2018-956308'),
 (9187,33,'Goa Psy Trance 2018 Top 100 Hits DJ Mix','2020-09-11 00:00:00',0,'https://39s.musify.club/img/68/22014725/56548575.jpg','https://musify.club/release/goa-psy-trance-2018-top-100-hits-dj-mix-2018-1373984'),
 (9188,33,'Psydelicious','2018-04-18 00:00:00',0,'https://41s-a.musify.club/img/70/16088993/42980771.jpg','https://musify.club/release/psydelicious-2017-985324'),
 (9189,33,'Goosebumps','2017-09-09 00:00:00',0,'https://41s-a.musify.club/img/70/14597090/37179744.jpg','https://musify.club/release/goosebumps-2017-903806'),
 (9190,33,'Medusa: Psy Goa Trance','2017-10-31 00:00:00',0,'https://41s-a.musify.club/img/71/14996271/38027361.jpg','https://musify.club/release/medusa-psy-goa-trance-2017-925213'),
 (9191,33,'Goa 2017 Vol. 1','2017-02-05 00:00:00',0,'https://41s.musify.club/img/68/12960028/33510686.jpg','https://musify.club/release/goa-2017-vol-1-2017-810988'),
 (9192,33,'Remixes','2018-04-06 00:00:00',0,'https://41s-a.musify.club/img/70/15997813/42804629.jpg','https://musify.club/release/remixes-2017-980290'),
 (9193,33,'Endless','2018-02-21 00:00:00',0,'https://40s-a.musify.club/img/70/15733349/42146503.jpg','https://musify.club/release/endless-2017-964790'),
 (9194,33,'Uncharted Vol. 5','2017-03-06 00:00:00',0,'https://41s.musify.club/img/69/13206140/34050372.jpg','https://musify.club/release/uncharted-vol-5-2017-825062'),
 (9195,33,'Selection Of The Finest Psytrance Chapter 3','2017-09-19 00:00:00',0,'https://41s-a.musify.club/img/71/14663703/37330171.jpg','https://musify.club/release/selection-of-the-finest-psytrance-chapter-3-2017-907715'),
 (9196,33,'A Brief History of Goa Trance','2017-06-05 00:00:00',0,'https://41s.musify.club/img/68/13923624/35712996.jpg','https://musify.club/release/a-brief-history-of-goa-trance-2017-860988'),
 (9197,33,'Goa 2017 Vol. 3','2017-10-05 00:00:00',0,'https://41s-a.musify.club/img/71/14798003/37603519.jpg','https://musify.club/release/goa-2017-vol-3-2017-914495'),
 (9198,33,'Goa Vol. 64','2017-10-05 00:00:00',0,'https://41s-a.musify.club/img/71/14794159/37601918.jpg','https://musify.club/release/goa-vol-64-2017-914418'),
 (9199,33,'Positive Visualisation','2017-12-31 00:00:00',0,'https://41s-a.musify.club/img/71/15440221/39002830.jpg','https://musify.club/release/talamasca-positive-visualisation-2017-948330'),
 (9200,33,'Дискотека 2017 Dance Club Vol. 160','2017-01-23 00:00:00',0,'/images/censored-l.jpg','https://musify.club/release/diskoteka-2017-dance-club-vol-160-2017-806300'),
 (9201,33,'Goa Trance Vol. 32','2016-06-20 00:00:00',0,'https://40s.musify.club/img/69/11616841/30643942.jpg','https://musify.club/release/goa-trance-vol-32-2016-736671'),
 (9202,33,'The Remixers, pt. 3 &quot;La French Connection&quot;','2018-07-26 00:00:00',0,'https://37s.musify.club/img/69/16752630/44475617.jpg','https://musify.club/release/the-remixers-pt-3-la-french-connection-2016-1023152'),
 (9203,33,'Highlights','2016-08-06 00:00:00',0,'https://38s-a.musify.club/img/70/11855437/31164778.jpg','https://musify.club/release/highlights-2016-749655'),
 (9204,33,'Goa 2016 Vol. 5','2016-12-05 00:00:00',0,'https://41s.musify.club/img/68/12572997/32690608.jpg','https://musify.club/release/goa-2016-vol-5-2016-789459'),
 (9205,33,'Goa Trance Vol. 31','2016-03-19 00:00:00',0,'https://37s-a.musify.club/img/71/11109517/29569818.jpg','https://musify.club/release/goa-trance-vol-31-2016-707051'),
 (9206,33,'Made in Brazil','2016-10-16 00:00:00',0,'https://38s-a.musify.club/img/70/12257084/32003691.jpg','https://musify.club/release/made-in-brazil-2016-772336'),
 (9207,33,'Goa Trance Vol. 33','2016-10-16 00:00:00',0,'https://38s-a.musify.club/img/70/12256677/32003660.jpg','https://musify.club/release/goa-trance-vol-33-2016-772358'),
 (9208,33,'Human Replicator- Psy Trance 2016','2017-01-29 00:00:00',0,'https://41s.musify.club/img/68/12920093/33419807.jpg','https://musify.club/release/human-replicator-psy-trance-2016-2016-808465'),
 (9209,33,'Goa Culture Vol. 21','2016-06-05 00:00:00',0,'https://39s.musify.club/img/68/11528198/30460111.jpg','https://musify.club/release/goa-culture-vol-21-2016-731767'),
 (9210,33,'Psy-Progressive Trance Vol. 1','2017-01-05 00:00:00',0,'https://41s.musify.club/img/69/12764503/33082513.jpg','https://musify.club/release/psy-progressive-trance-vol-1-2016-799688'),
 (9211,33,'Goa 2016 Vol 1','2016-01-18 00:00:00',0,'https://38s-a.musify.club/img/70/10648048/28287140.jpg','https://musify.club/release/goa-2016-vol-1-2016-679014'),
 (9212,33,'Boom F***ing Boom','2018-01-03 00:00:00',0,'https://41s-a.musify.club/img/71/15451428/39015252.jpg','https://musify.club/release/boom-f-ing-boom-2016-949222'),
 (9213,33,'Consciousness Determines Reality','2017-01-03 00:00:00',0,'https://41s.musify.club/img/68/12744926/33059433.jpg','https://musify.club/release/consciousness-determines-reality-2015-799356'),
 (9214,33,'Goatrance Heroes','2017-09-05 00:00:00',0,'https://41s-a.musify.club/img/71/14563435/37109279.jpg','https://musify.club/release/goatrance-heroes-2015-901939'),
 (9215,33,'Pacific Sound','2015-09-25 00:00:00',0,'https://38s.musify.club/img/69/10080706/26316743.jpg','https://musify.club/release/pacific-sound-2015-649854'),
 (9216,33,'Essentials volume 4','2015-09-04 00:00:00',0,'https://39s.musify.club/img/69/10003918/26141951.jpg','https://musify.club/release/essentials-volume-4-2015-645518'),
 (9217,33,'The Time Machine','2018-01-03 00:00:00',0,'https://41s-a.musify.club/img/71/15450919/39014612.jpg','https://musify.club/release/talamasca-the-time-machine-2015-949125'),
 (9218,33,'Global Trance Grooves 151 (13-10-2015) Javier Bussola Guestmix','2020-01-27 00:00:00',0,'https://38s.musify.club/img/69/20310249/52179529.jpg','https://musify.club/release/global-trance-grooves-151-13-10-2015-javier-bussola-guestmix-2015-1241512'),
 (9219,33,'Close Encounters Vol. 3','2015-04-30 00:00:00',0,'https://39s.musify.club/img/69/9405807/24931893.jpg','https://musify.club/release/close-encounters-vol-3-2015-617160'),
 (9220,33,'Level 9','2014-04-03 00:00:00',0,'https://40s-a.musify.club/img/70/6994339/19988026.jpg','https://musify.club/release/talamasca-level-9-2014-488003'),
 (9221,33,'Goa Trance 27','2014-11-03 00:00:00',0,'https://37s.musify.club/img/69/8355456/22617803.jpg','https://musify.club/release/goa-trance-27-2014-568939'),
 (9222,33,'Orientation Vol. 6','2014-10-18 00:00:00',0,'https://37s.musify.club/img/69/8209303/22416276.jpg','https://musify.club/release/orientation-vol-6-2014-564085'),
 (9223,33,'Mechanical Simulation','2014-10-15 00:00:00',0,'https://37s.musify.club/img/69/8181405/22366190.jpg','https://musify.club/release/mechanical-simulation-2014-552857'),
 (9224,33,'20 Year Anniversary','2014-09-28 00:00:00',0,'https://40s-a.musify.club/img/70/8104297/22163251.jpg','https://musify.club/release/20-year-anniversary-2014-548277'),
 (9225,33,'Progressive Techno DJ Mix 100 Hits 2014','2020-09-08 00:00:00',0,'https://39s.musify.club/img/68/21999533/56444548.jpg','https://musify.club/release/progressive-techno-dj-mix-100-hits-2014-2014-1372955'),
 (9226,33,'Space Gathering Compilation','2014-10-16 00:00:00',0,'https://37s.musify.club/img/69/8191577/22379345.jpg','https://musify.club/release/space-gathering-compilation-2014-553251'),
 (9227,33,'Progressive Goa Trance 101 Hits 2014 + DJ Mix','2020-10-10 00:00:00',0,'https://38s.musify.club/img/68/22274326/57541273.jpg','https://musify.club/release/progressive-goa-trance-101-hits-2014-dj-mix-2014-1385663'),
 (9228,33,'Psychedelic Trance','2013-05-07 00:00:00',0,'https://38s.musify.club/img/69/5053235/13261770.jpg','https://musify.club/release/psychedelic-trance-2013-380090'),
 (9229,33,'Goa 2013 Vol. 2','2013-05-11 00:00:00',0,'https://39s.musify.club/img/69/5083761/13969285.jpg','https://musify.club/release/goa-2013-vol-2-2013-380975'),
 (9230,33,'The Remixes','2017-07-09 00:00:00',0,'https://41s.musify.club/img/69/14118590/36150629.jpg','https://musify.club/release/the-remixes-2012-872568'),
 (9231,33,'Round 2','2017-05-04 00:00:00',0,'https://41s.musify.club/img/68/13692185/35253327.jpg','https://musify.club/release/round-2-2011-849593'),
 (9232,33,'SynSUN - Alter Ego','2011-06-05 00:00:00',0,'https://41s-a.musify.club/img/70/1170909/40218772.jpg','https://musify.club/release/synsun-alter-ego-2011-218224'),
 (9233,33,'Make Some Noise!','2011-08-01 00:00:00',0,'https://41s-a.musify.club/img/70/1229232/40159088.jpg','https://musify.club/release/make-some-noise-2011-237671'),
 (9234,33,'Goa Trip Vol. 3','2014-02-03 00:00:00',0,'https://37s-a.musify.club/img/68/6558012/27811532.jpg','https://musify.club/release/goa-trip-vol-3-2011-469794'),
 (9235,33,'TRANCELACIYA Vol 15 (PSY EDITION) [CD 1] Full On','2017-09-14 00:00:00',0,'https://41s-a.musify.club/img/71/14628147/37250139.jpg','https://musify.club/release/trancelaciya-vol-15-psy-edition-cd-1-full-on-2011-905763'),
 (9236,33,'Time In Desert','2017-03-02 00:00:00',0,'https://41s.musify.club/img/68/13171687/34009767.jpg','https://musify.club/release/time-in-desert-2011-822798'),
 (9237,33,'Back to Bach','2011-02-19 00:00:00',0,'https://41s-a.musify.club/img/70/1104236/40286245.jpg','https://musify.club/release/back-to-bach-2010-121259'),
 (9238,33,'No. 1','2017-05-11 00:00:00',0,'https://41s.musify.club/img/69/13753290/35371185.jpg','https://musify.club/release/no-1-2010-852635'),
 (9239,33,'TRANCELACIYA Vol. 14 (PSY EDITION) [CD 2] Psytrance &amp; Nightpsy','2017-09-14 00:00:00',0,'https://41s-a.musify.club/img/71/14627963/37250061.jpg','https://musify.club/release/trancelaciya-vol-14-psy-edition-cd-2-psytrance-and-nightpsy-2010-905762'),
 (9240,33,'Expander','2017-05-21 00:00:00',0,'https://41s.musify.club/img/68/13835441/35527499.jpg','https://musify.club/release/expander-2009-856646'),
 (9241,33,'One','2009-12-02 00:00:00',0,'https://41s-a.musify.club/img/71/15451399/39015207.jpg','https://musify.club/release/one-2009-53985'),
 (9242,33,'I Love Trance Paychedelic','2013-11-21 00:00:00',0,'https://39s-a.musify.club/img/70/6023838/18242825.jpg','https://musify.club/release/i-love-trance-paychedelic-2009-440119'),
 (9243,33,'Overload','2017-05-04 00:00:00',0,'https://41s.musify.club/img/69/13691335/35252536.jpg','https://musify.club/release/overload-2008-849595'),
 (9244,33,'Psychedelic Trance Vol.1','2015-04-29 00:00:00',0,'https://39s.musify.club/img/68/9385794/24912887.jpg','https://musify.club/release/psychedelic-trance-vol-1-2007-616599'),
 (9245,33,'Music With More Muscle','2008-03-30 00:00:00',0,'https://37s.musify.club/img/69/9330357/24686452.jpg','https://musify.club/release/music-with-more-muscle-2007-200712'),
 (9246,33,'Obsessive Dream','2008-09-05 00:00:00',0,'https://41s-a.musify.club/img/70/791600/40600704.jpg','https://musify.club/release/obsessive-dream-2007-20319'),
 (9247,33,'Illusion World','2018-01-03 00:00:00',0,'https://41s-a.musify.club/img/71/15451496/39015323.jpg','https://musify.club/release/talamasca-illusion-world-2005-949229'),
 (9248,33,'Psychedelic voice, Vol.1','2010-09-17 00:00:00',0,'https://41s-a.musify.club/img/70/1132446/40258133.jpg','https://musify.club/release/psychedelic-voice-vol-1-2005-205442'),
 (9249,33,'Made in Trance','2009-10-22 00:00:00',0,'https://41s-a.musify.club/img/71/15450644/39014318.jpg','https://musify.club/release/talamasca-made-in-trance-2004-47486'),
 (9250,33,'Attraction 4','2012-09-08 00:00:00',0,'https://39s.musify.club/img/69/3913201/15396585.jpg','https://musify.club/release/attraction-4-2004-326482'),
 (9251,33,'Zodiac','2009-10-14 00:00:00',0,'https://41s-a.musify.club/img/70/869264/40522345.jpg','https://musify.club/release/talamasca-zodiac-2003-46212'),
 (9252,33,'The Secret of the Thirteen Crystal Skulls','2018-01-03 00:00:00',0,'https://41s-a.musify.club/img/71/15451113/39014875.jpg','https://musify.club/release/the-secret-of-the-thirteen-crystal-skulls-2003-949237'),
 (9253,33,'Ticket To Goa Vol.1 (CD2)','2016-03-23 00:00:00',0,'https://38s-a.musify.club/img/70/11131862/29621433.jpg','https://musify.club/release/ticket-to-goa-vol-1-cd2-2002-708533'),
 (9254,33,'Goa-Head Vol.16 (CD2)','2016-03-19 00:00:00',0,'https://38s-a.musify.club/img/70/11082618/29555069.jpg','https://musify.club/release/goa-head-vol-16-cd2-2002-706750'),
 (9255,33,'3D Story - 2CD [CD1]','2015-03-05 00:00:00',0,'https://37s.musify.club/img/69/9066631/24186265.jpg','https://musify.club/release/3d-story-2cd-cd1-2002-602789'),
 (9256,33,'Global Psychedelic Trance Compilation Vol. 8','2016-02-25 00:00:00',0,'https://40s.musify.club/img/69/10952059/29246349.jpg','https://musify.club/release/global-psychedelic-trance-compilation-vol-8-2002-697968'),
 (9257,33,'Raja Ram&#39;s Stash Bag','2016-07-01 00:00:00',0,'https://38s-a.musify.club/img/70/11675185/30775121.jpg','https://musify.club/release/raja-rams-stash-bag-2002-740531'),
 (9258,33,'3D Story - 2CD [CD2]','2015-03-05 00:00:00',0,'https://39s-a.musify.club/img/71/9066767/24191127.jpg','https://musify.club/release/3d-story-2cd-cd2-2002-602820'),
 (9259,33,'Reborn','2015-03-06 00:00:00',0,'https://37s.musify.club/img/69/9073119/24204837.jpg','https://musify.club/release/reborn-2001-603144'),
 (9260,33,'Virtual Trance Vol. 2. Digital Alchemy','2012-12-01 00:00:00',0,'https://37s-a.musify.club/img/68/5421002/16319164.jpg','https://musify.club/release/virtual-trance-vol-2-digital-alchemy-2001-346860'),
 (9261,33,'Magnetic Fields','2018-01-03 00:00:00',0,'https://41s-a.musify.club/img/71/15451505/39015332.jpg','https://musify.club/release/talamasca-magnetic-fields-2001-949243'),
 (9262,33,'Musica Divinorum','2018-01-03 00:00:00',0,'https://41s-a.musify.club/img/71/15450363/39014002.jpg','https://musify.club/release/talamasca-musica-divinorum-2001-949133'),
 (9263,33,'Genetic Monster','2018-01-03 00:00:00',0,'https://41s-a.musify.club/img/71/15451510/39015340.jpg','https://musify.club/release/talamasca-genetic-monster-2001-949230'),
 (9264,33,'Ultimate Psychedelic: Psycho Tropic','2019-01-14 00:00:00',0,'https://40s.musify.club/img/69/17928938/47168319.jpg','https://musify.club/release/ultimate-psychedelic-psycho-tropic-2000-1093294'),
 (9265,33,'Future Navigators II - The Second Tour','2015-04-30 00:00:00',0,'https://39s.musify.club/img/69/9400661/24922587.jpg','https://musify.club/release/future-navigators-ii-the-second-tour-2000-616834'),
 (9266,33,'Beyond the Mask','2018-01-03 00:00:00',0,'https://41s-a.musify.club/img/71/15450060/39013652.jpg','https://musify.club/release/talamasca-beyond-the-mask-2000-949127'),
 (9267,33,'Pure MDMA Selected Vibes - Part 1','2015-03-14 00:00:00',0,'https://39s.musify.club/img/68/9118992/24290345.jpg','https://musify.club/release/pure-mdma-selected-vibes-part-1-1999-605216'),
 (9268,33,'Sina&#239; / Halloween','2018-01-03 00:00:00',0,'https://41s-a.musify.club/img/71/15451535/39015367.jpg','https://musify.club/release/talamasca-sinai-halloween-1999-949228'),
 (9269,33,'This Is... Goa Vol. 2 - The Best Of Goa-Head [CD1]','2022-04-04 00:00:00',0,'https://40s-a.musify.club/img/70/25512179/64058375.jpg','https://musify.club/release/this-is-goa-vol-2-the-best-of-goa-head-cd1-1998-1607025'),
 (9270,33,'Future Navigators','2015-04-17 00:00:00',0,'https://37s.musify.club/img/69/9306001/24659352.jpg','https://musify.club/release/future-navigators-1998-613855'),
 (9271,33,'Pure MDMA','2021-03-13 00:00:00',0,'https://40s.musify.club/img/68/23399666/59646309.jpg','https://musify.club/release/pure-mdma-1998-1460606'),
 (9272,33,'Psychic Orgasm Vol. 1 - Psychedelic Trance Mission','2015-07-21 00:00:00',0,'https://39s-a.musify.club/img/71/9805899/25684237.jpg','https://musify.club/release/psychic-orgasm-vol-1-psychedelic-trance-mission-1998-635184'),
 (9273,33,'Goa-Head Vol. 3','2011-12-19 00:00:00',0,'https://39s.musify.club/img/69/1991218/15400773.jpg','https://musify.club/release/goa-head-vol-3-1997-268799'),
 (9274,33,'Euro Trance Euro Dance Vol.6','2013-01-10 00:00:00',0,'https://39s.musify.club/img/69/4415173/15363507.jpg','https://musify.club/release/euro-trance-euro-dance-vol-6-355415'),
 (9275,32,'Space Psychedelic Trance','2022-03-26 00:00:00',0,'https://41s-a.musify.club/img/71/25471116/63920521.jpg','https://musify.club/release/space-psychedelic-trance-2022-1602942'),
 (9276,32,'The Rising Of Kukulkan','2021-03-29 00:00:00',0,'https://38s.musify.club/img/68/23512532/59882136.jpg','https://musify.club/release/the-rising-of-kukulkan-2021-1468962'),
 (9277,32,'Beatport Psy Trance. Sound Pack #321','2021-10-06 00:00:00',0,'https://37s.musify.club/img/69/24537813/62308584.jpg','https://musify.club/release/beatport-psy-trance-sound-pack-321-2021-1544994'),
 (9278,32,'Beatport Psy Trance. Electro Sound Pack #306','2021-07-04 00:00:00',0,'https://39s.musify.club/img/68/24212961/61352118.jpg','https://musify.club/release/beatport-psy-trance-electro-sound-pack-306-2021-1518086'),
 (9279,32,'Beatport Psychedelic Trance. Sound Pack #316','2021-07-08 00:00:00',0,'https://38s.musify.club/img/68/24233087/61398360.jpg','https://musify.club/release/beatport-psychedelic-trance-sound-pack-316-2021-1520078'),
 (9280,32,'Classy Psy Trance. White Session','2021-07-20 00:00:00',0,'https://40s.musify.club/img/69/24288991/61521074.jpg','https://musify.club/release/classy-psy-trance-white-session-2021-1523215'),
 (9281,32,'Another Galaxy','2021-05-01 00:00:00',0,'https://37s-a.musify.club/img/68/23769231/60421119.jpg','https://musify.club/release/median-project-another-galaxy-2021-1486301'),
 (9282,32,'Free Choice, Vol. 3','2022-01-25 00:00:00',0,'https://41s-a.musify.club/img/71/25031890/63079093.jpg','https://musify.club/release/median-project-free-choice-vol-3-2021-1570316'),
 (9283,32,'Beatport Psychedelic Trance: Sound Pack #349','2022-01-19 00:00:00',0,'https://41s.musify.club/img/69/24671217/62965551.jpg','https://musify.club/release/beatport-psychedelic-trance-sound-pack-349-2021-1565922'),
 (9284,32,'Beatport Psychedelic Trance. Sound Pack #62','2020-05-13 00:00:00',0,'https://38s-a.musify.club/img/70/21077628/53805253.jpg','https://musify.club/release/beatport-psychedelic-trance-sound-pack-62-2020-1288244'),
 (9285,32,'Globalsect Radio','2020-08-01 00:00:00',0,'https://40s-a.musify.club/img/71/21734058/55388901.jpg','https://musify.club/release/globalsect-radio-2020-1329242'),
 (9286,32,'Goa Trance Wizards, vol. 1','2020-02-09 00:00:00',0,'https://39s-a.musify.club/img/71/20654195/52850468.jpg','https://musify.club/release/goa-trance-wizards-vol-1-2020-1248947'),
 (9287,32,'Free Choice','2020-11-05 00:00:00',0,'https://38s-a.musify.club/img/71/22472990/57751908.jpg','https://musify.club/release/median-project-free-choice-2020-1396601'),
 (9288,32,'Free Choice, Vol.2','2020-11-05 00:00:00',0,'https://38s-a.musify.club/img/71/22473011/57751935.jpg','https://musify.club/release/median-project-free-choice-vol-2-2020-1396622'),
 (9289,32,'Warp Duality • A Goa Trance Love Story','2020-03-09 00:00:00',0,'https://40s-a.musify.club/img/71/20678980/52926100.jpg','https://musify.club/release/warp-duality-a-goa-trance-love-story-2020-1260767'),
 (9290,32,'Voyager: Third Plateau','2020-08-01 00:00:00',0,'https://40s-a.musify.club/img/71/21734102/55388958.jpg','https://musify.club/release/voyager-third-plateau-2020-1329243'),
 (9291,32,'Goa Trance Voyage, vol. 1','2020-02-09 00:00:00',0,'https://39s-a.musify.club/img/71/20395069/52400900.jpg','https://musify.club/release/goa-trance-voyage-vol-1-2020-1248955'),
 (9292,32,'Beatport Psychedelic Trance. Electro Sound Pack #44 CD1','2020-04-08 00:00:00',0,'https://38s.musify.club/img/69/20817108/53244503.jpg','https://musify.club/release/beatport-psychedelic-trance-electro-sound-pack-44-cd1-2020-1272673'),
 (9293,32,'The Call of Goa, vol. 4','2020-11-08 00:00:00',0,'https://38s.musify.club/img/68/22490244/57773443.jpg','https://musify.club/release/the-call-of-goa-vol-4-2020-1397785'),
 (9294,32,'Nostromosis @ Work','2020-05-14 00:00:00',0,'https://37s-a.musify.club/img/68/21091678/53834129.jpg','https://musify.club/release/nostromosis-work-2020-1289077'),
 (9295,32,'Goa Trance 2020, vol. 1','2020-02-09 00:00:00',0,'https://39s-a.musify.club/img/70/20370785/52396582.jpg','https://musify.club/release/goa-trance-2020-vol-1-2020-1248792'),
 (9296,32,'Music Of The Future # 1','2020-02-22 00:00:00',0,'https://40s-a.musify.club/img/71/20497260/52595634.jpg','https://musify.club/release/music-of-the-future-1-2020-1255300'),
 (9297,32,'Symphonix Psychedelic Goa Trance','2020-03-16 00:00:00',0,'https://39s.musify.club/img/69/20631751/52884650.jpg','https://musify.club/release/symphonix-psychedelic-goa-trance-2019-1262437'),
 (9298,32,'Constellation','2019-05-26 00:00:00',0,'https://40s-a.musify.club/img/71/18837768/49042919.jpg','https://musify.club/release/median-project-constellation-2019-1149124'),
 (9299,32,'Goa Trance Chronicles, v.1','2019-07-29 00:00:00',0,'https://40s-a.musify.club/img/71/19260182/49940896.jpg','https://musify.club/release/goa-trance-chronicles-v-1-2019-1179241'),
 (9300,32,'Goa Trance Festival 2019','2019-07-29 00:00:00',0,'https://40s-a.musify.club/img/71/19265858/49947263.jpg','https://musify.club/release/goa-trance-festival-2019-2019-1179564'),
 (9301,32,'Resonant Reality. Trance Psychedelic Party','2019-09-15 00:00:00',0,'https://40s-a.musify.club/img/71/19569464/50592477.jpg','https://musify.club/release/resonant-reality-trance-psychedelic-party-2019-1199045'),
 (9302,32,'Goa 2020 Top 40 Hits','2019-12-17 00:00:00',0,'https://38s-a.musify.club/img/70/20140496/51811186.jpg','https://musify.club/release/goa-2020-top-40-hits-2019-1233761'),
 (9303,32,'Psychedelic Goa Trance 2019','2020-12-06 00:00:00',0,'https://38s.musify.club/img/68/22701118/58177619.jpg','https://musify.club/release/psychedelic-goa-trance-2019-2019-1410114'),
 (9304,32,'Astronauts in the Solar System','2019-12-20 00:00:00',0,'https://39s-a.musify.club/img/70/20163887/51846550.jpg','https://musify.club/release/astronauts-in-the-solar-system-2019-1234897'),
 (9305,32,'The 50th Parallel','2018-12-02 00:00:00',0,'https://37s-a.musify.club/img/71/17655356/46431619.jpg','https://musify.club/release/the-50th-parallel-2018-1074293'),
 (9306,32,'Colors of Goa, v.3','2018-09-08 00:00:00',0,'https://40s-a.musify.club/img/70/17070351/45141468.jpg','https://musify.club/release/colors-of-goa-v-3-2018-1040858'),
 (9307,32,'In the Depths of Space','2018-02-01 00:00:00',0,'https://41s-a.musify.club/img/71/15622033/39486186.jpg','https://musify.club/release/in-the-depths-of-space-2018-958387'),
 (9308,32,'Shambhala','2018-10-02 00:00:00',0,'https://38s.musify.club/img/69/17259582/45575224.jpg','https://musify.club/release/shambhala-2018-1051480'),
 (9309,32,'303 Syndroms','2018-10-12 00:00:00',0,'https://39s-a.musify.club/img/70/17333353/45719422.jpg','https://musify.club/release/303-syndroms-2018-1055049'),
 (9310,32,'The Call of Goa, vol. 3: New Horizons','2018-02-27 00:00:00',0,'https://40s-a.musify.club/img/70/15771552/42302982.jpg','https://musify.club/release/the-call-of-goa-vol-3-new-horizons-2018-966421'),
 (9311,32,'My Way (The Next Step)','2018-10-12 00:00:00',0,'https://39s.musify.club/img/69/17337382/45726656.jpg','https://musify.club/release/my-way-the-next-step-2018-1055131'),
 (9312,32,'Techno Trance 2018 100 Hits DJ Mix','2020-07-21 00:00:00',0,'https://38s.musify.club/img/69/21628482/54896107.jpg','https://musify.club/release/techno-trance-2018-100-hits-dj-mix-2018-1319971'),
 (9313,32,'Progressive Goa Trance 2018 Top 100 Hits DJ Mix Part 1','2020-09-11 00:00:00',0,'https://39s.musify.club/img/68/22014747/56548612.jpg','https://musify.club/release/progressive-goa-trance-2018-top-100-hits-dj-mix-part-1-2018-1373986'),
 (9314,32,'Psy Goa Trance. Beach Party','2019-02-18 00:00:00',0,'https://39s.musify.club/img/69/18178160/47732664.jpg','https://musify.club/release/psy-goa-trance-beach-party-2018-1107650'),
 (9315,32,'Goa Trance Timewarp, vol.4','2018-11-14 00:00:00',0,'https://37s-a.musify.club/img/68/17552498/46223896.jpg','https://musify.club/release/goa-trance-timewarp-vol-4-2018-1069016'),
 (9316,32,'Elan Vital','2018-11-10 00:00:00',0,'https://37s-a.musify.club/img/68/17530074/46188382.jpg','https://musify.club/release/elan-vital-2018-1067258'),
 (9317,32,'Goa Psy Trance 2018 Top 100 Hits DJ Mix','2020-09-11 00:00:00',0,'https://39s.musify.club/img/68/22014725/56548575.jpg','https://musify.club/release/goa-psy-trance-2018-top-100-hits-dj-mix-2018-1373984'),
 (9318,32,'The Metasphere','2020-03-24 00:00:00',0,'https://40s-a.musify.club/img/71/20791845/53152154.jpg','https://musify.club/release/the-metasphere-2018-1266032'),
 (9319,32,'The Wanderer','2017-07-28 00:00:00',0,'https://41s.musify.club/img/68/14266134/36442749.jpg','https://musify.club/release/median-project-the-wanderer-2017-881195'),
 (9320,32,'Goa Trance Timewarp Vol. 3','2017-11-29 00:00:00',0,'https://41s-a.musify.club/img/71/15227729/38524341.jpg','https://musify.club/release/goa-trance-timewarp-vol-3-2017-937463'),
 (9321,32,'On Saturn','2017-12-09 00:00:00',0,'https://41s-a.musify.club/img/71/15298899/38668366.jpg','https://musify.club/release/on-saturn-2017-940763'),
 (9322,32,'Space Factory','2019-05-30 00:00:00',0,'https://40s-a.musify.club/img/71/18860667/49107324.jpg','https://musify.club/release/median-project-space-factory-2017-1151274'),
 (9323,32,'Terraformer','2017-06-03 00:00:00',0,'https://41s.musify.club/img/68/13906743/35684174.jpg','https://musify.club/release/terraformer-2017-860171'),
 (9324,32,'Timeless Heart Mix Psy Trance Story','2017-10-23 00:00:00',0,'https://41s-a.musify.club/img/71/14928052/37892832.jpg','https://musify.club/release/timeless-heart-mix-psy-trance-story-2017-921877'),
 (9325,32,'Selection Of The Finest Psytrance Chapter 2','2017-09-22 00:00:00',0,'https://41s-a.musify.club/img/71/14663699/37360760.jpg','https://musify.club/release/selection-of-the-finest-psytrance-chapter-2-2017-908253'),
 (9326,32,'Island Peaces Bright Worlds','2015-04-24 00:00:00',0,'https://39s-a.musify.club/img/71/9328930/24752012.jpg','https://musify.club/release/island-peaces-bright-worlds-2015-615199'),
 (9327,32,'Space Time','2019-05-30 00:00:00',0,'https://40s-a.musify.club/img/71/18860530/49107306.jpg','https://musify.club/release/median-project-space-time-2015-1151273'),
 (9328,30,'Navras (Remixes)','2022-01-19 00:00:00',0,'https://41s.musify.club/img/68/25096787/62973961.jpg','https://musify.club/release/juno-reactor-navras-remixes-2022-1566151'),
 (9329,30,'Inside The Upside Down','2022-03-18 00:00:00',0,'https://37s.musify.club/img/69/25419017/63820947.jpg','https://musify.club/release/inside-the-upside-down-2022-1600441'),
 (9330,30,'Works By Juno Reactor','2020-04-10 00:00:00',0,'https://38s-a.musify.club/img/70/20735175/53290345.jpg','https://musify.club/release/works-by-juno-reactor-2020-1274017'),
 (9331,30,'Euro Trance Euro Dance Vol.102','2020-05-06 00:00:00',0,'https://39s-a.musify.club/img/71/21017237/53692333.jpg','https://musify.club/release/euro-trance-euro-dance-vol-102-2020-1284619'),
 (9332,30,'Psychedelic Selections, vol. 005','2020-04-13 00:00:00',0,'https://40s-a.musify.club/img/71/20854825/53325823.jpg','https://musify.club/release/psychedelic-selections-vol-005-2020-1274969'),
 (9333,30,'All About GMS','2020-02-10 00:00:00',0,'https://39s-a.musify.club/img/71/20413191/52415047.jpg','https://musify.club/release/all-about-gms-2020-1249398'),
 (9334,30,'Goa Trance Psy Trance (2019)','2019-10-28 00:00:00',0,'https://40s-a.musify.club/img/71/19875418/51237564.jpg','https://musify.club/release/goa-trance-psy-trance-2019-2019-1218700'),
 (9335,30,'Into Valhalla','2019-08-11 00:00:00',0,'https://40s-a.musify.club/img/71/19354624/50142646.jpg','https://musify.club/release/into-valhalla-2019-1185134'),
 (9336,30,'The Ubar Tmar Works','2019-11-18 00:00:00',0,'https://39s.musify.club/img/69/19993488/51540026.jpg','https://musify.club/release/the-ubar-tmar-works-2019-1227460'),
 (9337,30,'Goa Beach, Vol. 2 - Psytrance In Paradise','2019-09-04 00:00:00',0,'https://40s-a.musify.club/img/71/19465850/50417194.jpg','https://musify.club/release/goa-beach-vol-2-psytrance-in-paradise-2019-1192417'),
 (9338,30,'Ingonyama [BLiSS Remix]','2019-06-10 00:00:00',0,'https://40s-a.musify.club/img/71/18953555/49275184.jpg','https://musify.club/release/ingonyama-bliss-remix-2019-1156135'),
 (9339,30,'Set: 33','2019-07-28 00:00:00',0,'https://40s-a.musify.club/img/71/19254958/49929634.jpg','https://musify.club/release/set-33-2019-1178935'),
 (9340,30,'Psy Trance Psychedelic (2019)','2019-10-29 00:00:00',0,'https://38s-a.musify.club/img/70/19886910/51266264.jpg','https://musify.club/release/psy-trance-psychedelic-2019-2019-1219427'),
 (9341,30,'Psy-Nation Radio #019','2019-07-28 00:00:00',0,'https://40s-a.musify.club/img/71/19251016/49924098.jpg','https://musify.club/release/psy-nation-radio-019-2019-1178693'),
 (9342,30,'Psy-Nation Radio #008','2018-08-30 00:00:00',0,'https://38s-a.musify.club/img/70/17005198/45042449.jpg','https://musify.club/release/psy-nation-radio-008-2018-1036715'),
 (9343,30,'Zwara [30Trip Remix]','2019-01-24 00:00:00',0,'https://37s.musify.club/img/69/17988888/47242893.jpg','https://musify.club/release/juno-reactor-zwara-30trip-remix-2018-1096365'),
 (9344,30,'Клубная Вечеринка CD2 ','2019-10-29 00:00:00',0,'https://38s-a.musify.club/img/70/19885457/51264235.jpg','https://musify.club/release/klubnaya-vecherinka-cd2-2018-1219370'),
 (9345,30,'Клубная Вечеринка Vol.2 CD2','2019-10-29 00:00:00',0,'https://40s-a.musify.club/img/71/19883956/51260681.jpg','https://musify.club/release/klubnaya-vecherinka-vol-2-cd2-2018-1219311'),
 (9346,30,'Клубная Вечеринка Vol.3 CD2','2019-10-29 00:00:00',0,'https://40s-a.musify.club/img/71/19883315/51260136.jpg','https://musify.club/release/klubnaya-vecherinka-vol-3-cd2-2018-1219295'),
 (9347,30,'Клубная Вечеринка Vol.2 CD1','2019-10-29 00:00:00',0,'https://40s-a.musify.club/img/71/19883948/51260559.jpg','https://musify.club/release/klubnaya-vecherinka-vol-2-cd1-2018-1219310'),
 (9348,30,'Клубная Вечеринка Vol.3 CD1','2019-10-29 00:00:00',0,'https://40s-a.musify.club/img/71/19883312/51260011.jpg','https://musify.club/release/klubnaya-vecherinka-vol-3-cd1-2018-1219294'),
 (9349,30,'Komit [3 of Life vs. Domestic Remix]','2018-05-17 00:00:00',0,'https://41s-a.musify.club/img/70/16271833/43359464.jpg','https://musify.club/release/juno-reactor-komit-3-of-life-vs-domestic-remix-2018-994696'),
 (9350,30,'Dakota','2018-08-15 00:00:00',0,'https://40s-a.musify.club/img/70/16906321/44767876.jpg','https://musify.club/release/juno-reactor-dakota-2018-1031078'),
 (9351,30,'The Mutant Theatre','2018-06-21 00:00:00',0,'https://40s.musify.club/img/69/16521560/43894580.jpg','https://musify.club/release/juno-reactor-the-mutant-theatre-2018-1009677'),
 (9352,30,'Trance 16','2019-02-15 00:00:00',0,'https://39s-a.musify.club/img/70/18155726/47704018.jpg','https://musify.club/release/trance-16-2018-1105889'),
 (9353,30,'Hommega 20 Aniversary','2018-09-02 00:00:00',0,'https://38s-a.musify.club/img/70/16984464/45013914.jpg','https://musify.club/release/hommega-20-aniversary-2017-1038504'),
 (9354,30,'Cyber Ninja Vol. 6','2017-10-31 00:00:00',0,'https://41s-a.musify.club/img/71/14992529/38012554.jpg','https://musify.club/release/cyber-ninja-vol-6-2017-924644'),
 (9355,30,'Harmonize','2017-03-19 00:00:00',0,'https://41s.musify.club/img/69/13331339/34275818.jpg','https://musify.club/release/harmonize-2017-830214'),
 (9356,30,'Our World','2017-11-19 00:00:00',0,'https://41s-a.musify.club/img/71/15149232/38359211.jpg','https://musify.club/release/our-world-2017-933566'),
 (9357,30,'Ash968 Presents Psy World Vol. 2','2017-08-06 00:00:00',0,'https://41s.musify.club/img/68/14335348/36583185.jpg','https://musify.club/release/ash968-presents-psy-world-vol-2-2017-886487'),
 (9358,30,'Mirror Of Illusion','2017-10-03 00:00:00',0,'https://41s-a.musify.club/img/71/14782716/37580788.jpg','https://musify.club/release/mirror-of-illusion-2017-914087'),
 (9359,30,'DJ Box May 2016','2016-06-21 00:00:00',0,'https://40s.musify.club/img/69/11624584/30658112.jpg','https://musify.club/release/dj-box-may-2016-2016-737162'),
 (9360,30,'Global Trance Grooves 161 (09-08-2016) Shugz Guestmix','2020-01-27 00:00:00',0,'https://39s-a.musify.club/img/70/20312203/52180373.jpg','https://musify.club/release/global-trance-grooves-161-09-08-2016-shugz-guestmix-2016-1241537'),
 (9361,30,'Lazarus Rising','2016-08-23 00:00:00',0,'https://37s.musify.club/img/69/11954808/31376785.jpg','https://musify.club/release/lazarus-rising-2016-754860'),
 (9362,30,'Regenerate X','2016-08-24 00:00:00',0,'https://40s-a.musify.club/img/70/11960657/31391935.jpg','https://musify.club/release/regenerate-x-2016-755401'),
 (9363,30,'Psy-Progressive Trance Vol. 1','2017-01-05 00:00:00',0,'https://41s.musify.club/img/69/12764503/33082513.jpg','https://musify.club/release/psy-progressive-trance-vol-1-2016-799688'),
 (9364,30,'He.Art','2016-03-01 00:00:00',0,'https://37s-a.musify.club/img/71/10983426/29306895.jpg','https://musify.club/release/he-art-2016-699503'),
 (9365,30,'ReVertiGo 2','2015-10-25 00:00:00',0,'https://37s.musify.club/img/69/10219902/26622663.jpg','https://musify.club/release/revertigo-2-2015-656807'),
 (9366,30,'Summer Solstice &#39;2015','2015-06-28 00:00:00',0,'https://39s.musify.club/img/68/9697928/25487000.jpg','https://musify.club/release/summer-solstice-2015-2015-629700'),
 (9367,30,'The Golden Sun... Remixed','2015-05-27 00:00:00',0,'https://39s-a.musify.club/img/71/9555024/25185594.jpg','https://musify.club/release/juno-reactor-the-golden-sun-remixed-2015-622926'),
 (9368,30,'Paradise Engineering','2015-05-21 00:00:00',0,'https://37s.musify.club/img/69/9529918/25128491.jpg','https://musify.club/release/paradise-engineering-2015-621736'),
 (9369,30,'Omvision Pt.33','2015-10-07 00:00:00',0,'https://38s.musify.club/img/69/10142179/26450805.jpg','https://musify.club/release/omvision-pt-33-2015-652466'),
 (9370,30,'Ritmo. Some Kind of Rhythm 003','2014-10-18 00:00:00',0,'https://37s.musify.club/img/69/8209895/22416564.jpg','https://musify.club/release/ritmo-some-kind-of-rhythm-003-2014-564096'),
 (9371,30,'RadiOzora Mix 17 Feb 2014','2014-06-04 00:00:00',0,'https://40s.musify.club/img/69/7423573/20854941.jpg','https://musify.club/release/radiozora-mix-17-feb-2014-2014-509419'),
 (9372,30,'Wind Of Buri - Movie Magic 012 - Orchestral Mix','2020-03-15 00:00:00',0,'https://38s-a.musify.club/img/70/20609122/52853175.jpg','https://musify.club/release/wind-of-buri-movie-magic-012-orchestral-mix-2014-1261056'),
 (9373,30,'Mortal Kombat: Devastation','2014-07-01 00:00:00',0,'https://38s.musify.club/img/69/7568915/21164986.jpg','https://musify.club/release/mortal-kombat-devastation-2014-517107'),
 (9374,30,'Global Trance Grooves 121 (30-04-2013) Part10 Micky Noise','2020-01-20 00:00:00',0,'https://38s-a.musify.club/img/70/20301076/52124593.jpg','https://musify.club/release/global-trance-grooves-121-30-04-2013-part10-micky-noise-2013-1240858'),
 (9375,30,'Final Frontier','2013-04-29 00:00:00',0,'https://38s.musify.club/img/69/5027907/13227282.jpg','https://musify.club/release/juno-reactor-final-frontier-2013-379249'),
 (9376,30,'Techno Club Vol.43 [CD 2]','2014-01-04 00:00:00',0,'https://39s-a.musify.club/img/70/6396741/18844030.jpg','https://musify.club/release/techno-club-vol-43-cd-2-2013-460847'),
 (9377,30,'Best 2012 House Mix','2013-11-17 00:00:00',0,'https://39s-a.musify.club/img/70/6005913/18211416.jpg','https://musify.club/release/best-2012-house-mix-2013-439315'),
 (9378,30,'The Matrix Reloaded - OST/ Матрица: Перезагрузка - Саундтрек [Expanded Score]','2020-12-16 00:00:00',0,'https://38s.musify.club/img/68/22785522/58344021.jpg','https://musify.club/release/the-matrix-reloaded-ost-matritsa-perezagruzka-saundtrek-expanded-score-2013-1415273'),
 (9379,30,'The Golden Sun Of The Great East','2013-04-29 00:00:00',0,'https://38s.musify.club/img/69/5027370/13226544.jpg','https://musify.club/release/juno-reactor-the-golden-sun-of-the-great-east-2013-379128'),
 (9380,30,'Global Trance Grooves 113 (11-09-2012) Reaky Guestmix','2020-01-19 00:00:00',0,'https://39s-a.musify.club/img/71/20299802/52123213.jpg','https://musify.club/release/global-trance-grooves-113-11-09-2012-reaky-guestmix-2012-1240812'),
 (9381,30,'Ubarpedia CD 2','2019-11-20 00:00:00',0,'https://40s-a.musify.club/img/71/20000524/51564021.jpg','https://musify.club/release/ubarpedia-cd-2-2012-1228128'),
 (9382,30,'Mortal Kombat: Choose Your Destiny','2012-10-17 00:00:00',0,'https://39s-a.musify.club/img/70/7799274/21597921.jpg','https://musify.club/release/mortal-kombat-choose-your-destiny-2012-337467'),
 (9383,30,'Mortal Kombat: Trilogy In Dance','2015-05-20 00:00:00',0,'https://39s.musify.club/img/69/9519101/25116756.jpg','https://musify.club/release/mortal-kombat-trilogy-in-dance-2012-621352'),
 (9384,30,'Future Kids Material','2017-11-11 00:00:00',0,'https://41s-a.musify.club/img/71/15073750/38204942.jpg','https://musify.club/release/future-kids-material-2012-930175'),
 (9385,30,'Trance Pro V.3','2014-04-29 00:00:00',0,'https://40s-a.musify.club/img/70/7164169/20342940.jpg','https://musify.club/release/trance-pro-v-3-2012-497366'),
 (9386,30,'Halloween Party Mix (Kiruz Compilation)','2012-11-23 00:00:00',0,'https://39s.musify.club/img/69/4219094/15503067.jpg','https://musify.club/release/halloween-party-mix-kiruz-compilation-2012-344826'),
 (9387,30,'Жажда Скорости Сборник Лучшего Vol. 02','2020-05-16 00:00:00',0,'https://39s-a.musify.club/img/71/21118829/53877623.jpg','https://musify.club/release/zhazhda-skorosti-sbornik-luchshego-vol-02-2012-1290571'),
 (9388,30,'From The Land Of The Rising Sun-Inside The Reactor II','2013-01-29 00:00:00',0,'https://39s.musify.club/img/69/4575183/15321281.jpg','https://musify.club/release/juno-reactor-from-the-land-of-the-rising-sun-inside-the-reactor-ii-2012-359946'),
 (9389,30,'Inside The Reactor','2011-07-25 00:00:00',0,'https://41s-a.musify.club/img/70/1222731/40165616.jpg','https://musify.club/release/juno-reactor-inside-the-reactor-2011-235505'),
 (9390,30,'Audioception Disc2: Versus Satanicus','2011-09-30 00:00:00',0,'https://41s-a.musify.club/img/70/1260465/40127569.jpg','https://musify.club/release/audioception-disc2-versus-satanicus-2011-248081'),
 (9391,30,'The History Of Trance Euphoria','2015-07-07 00:00:00',0,'https://37s.musify.club/img/69/9734267/25556688.jpg','https://musify.club/release/the-history-of-trance-euphoria-2010-631499'),
 (9392,30,'Metropolis: Rebirth 2.0','2015-12-20 00:00:00',0,'https://39s.musify.club/img/69/10487729/27230734.jpg','https://musify.club/release/metropolis-rebirth-2-0-2009-672273'),
 (9393,30,'Gods &amp; Monsters','2008-08-27 00:00:00',0,'https://41s-a.musify.club/img/70/790100/40602198.jpg','https://musify.club/release/juno-reactor-gods-and-monsters-2008-19819'),
 (9394,30,'Psychedelic Trance Vol.1','2015-04-29 00:00:00',0,'https://39s.musify.club/img/68/9385794/24912887.jpg','https://musify.club/release/psychedelic-trance-vol-1-2007-616599'),
 (9395,30,'MP3 Collection: Progressive Soundtrack','2015-04-14 00:00:00',0,'https://39s-a.musify.club/img/71/9293817/24632116.jpg','https://musify.club/release/mp3-collection-progressive-soundtrack-2007-613286'),
 (9396,30,'Septic VI','2015-07-28 00:00:00',0,'https://39s-a.musify.club/img/71/9702243/25755733.jpg','https://musify.club/release/septic-vi-2006-636887'),
 (9397,30,'Labyrinth','2007-08-23 00:00:00',0,'https://40s.musify.club/img/69/703027/13352973.jpg','https://musify.club/release/juno-reactor-labyrinth-2004-4664'),
 (9398,30,'Once upon a time in Mexico - OST / Однажды в Мексике: Отчаянный 2 - Саундтрек','2006-12-24 00:00:00',0,'https://41s-a.musify.club/img/70/1116360/40274200.jpg','https://musify.club/release/once-upon-a-time-in-mexico-ost-odnazhdi-v-meksike-otchayannii-2-saundt-2003-200076'),
 (9399,30,'Back To Mine: The Orb','2016-07-28 00:00:00',0,'https://39s.musify.club/img/69/11790907/31026799.jpg','https://musify.club/release/back-to-mine-the-orb-2003-747157'),
 (9400,30,'The Zwara','2010-05-11 00:00:00',0,'https://41s-a.musify.club/img/70/944627/40446808.jpg','https://musify.club/release/juno-reactor-the-zwara-2003-71335'),
 (9401,30,'The Rock Matrix','2021-01-06 00:00:00',0,'https://38s.musify.club/img/68/22919033/58647584.jpg','https://musify.club/release/the-rock-matrix-2003-1424345'),
 (9402,30,'The Matrix: Reloaded - OST / Матрица: Перезагрузка - Саундтрек [Score]','2010-09-22 00:00:00',0,'https://41s.musify.club/img/68/12964056/33498395.jpg','https://musify.club/release/the-matrix-reloaded-ost-matritsa-perezagruzka-saundtrek-score-2003-98083'),
 (9403,30,'The Matrix: Reloaded - OST / Матрица: Перезагрузка - Саундтрек','2007-10-31 00:00:00',0,'https://41s-a.musify.club/img/70/1117065/40273494.jpg','https://musify.club/release/the-matrix-reloaded-ost-matritsa-perezagruzka-saundtrek-2003-200319'),
 (9404,30,'Enter The Matrix - OST','2008-03-08 00:00:00',0,'https://41s-a.musify.club/img/70/1117920/40272641.jpg','https://musify.club/release/enter-the-matrix-ost-2003-200607'),
 (9405,30,'Matrix Soundtrack','2018-03-28 00:00:00',0,'https://41s-a.musify.club/img/70/15901281/42679941.jpg','https://musify.club/release/matrix-soundtrack-2003-976656'),
 (9406,30,'Odyssey 1992-2002','2012-02-23 00:00:00',0,'https://39s.musify.club/img/69/3246059/15437660.jpg','https://musify.club/release/juno-reactor-odyssey-1992-2002-2003-285366'),
 (9407,30,'The Animatrix - OST / Аниматрица - Саундтрек','2010-03-19 00:00:00',0,'https://41s-a.musify.club/img/70/1125642/40264928.jpg','https://musify.club/release/the-animatrix-ost-animatritsa-saundtrek-2003-203176'),
 (9408,30,'Progressive Soundtrack CD7 - Killer Instinct','2014-10-16 00:00:00',0,'https://39s-a.musify.club/img/71/8187405/22380383.jpg','https://musify.club/release/progressive-soundtrack-cd7-killer-instinct-2003-553300'),
 (9409,30,'Hotaka','2008-02-10 00:00:00',0,'https://37s.musify.club/img/69/9616324/25272829.jpg','https://musify.club/release/juno-reactor-hotaka-2002-10121'),
 (9410,30,'Progressive Soundtrack CD1 - Under Attack','2014-10-16 00:00:00',0,'https://39s-a.musify.club/img/71/8186317/22381220.jpg','https://musify.club/release/progressive-soundtrack-cd1-under-attack-2002-553541'),
 (9411,30,'Masters of the Universe','2008-01-11 00:00:00',0,'https://40s.musify.club/img/69/715996/13337544.jpg','https://musify.club/release/juno-reactor-masters-of-the-universe-2001-8592'),
 (9412,30,'Ambient Meditations 3','2021-04-12 00:00:00',0,'https://40s.musify.club/img/68/23643117/60161180.jpg','https://musify.club/release/ambient-meditations-3-2000-1478119'),
 (9413,30,'Shango','2008-01-07 00:00:00',0,'https://41s-a.musify.club/img/70/715571/40667346.jpg','https://musify.club/release/juno-reactor-shango-2000-8470'),
 (9414,30,'Beowulf OST / Беовульф - Саундтрек ','2011-11-19 00:00:00',0,'https://41s-a.musify.club/img/70/1051865/40338668.jpg','https://musify.club/release/beowulf-ost-beovulf-saundtrek-1999-258266'),
 (9415,30,'Pistolero','2008-01-20 00:00:00',0,'https://39s-a.musify.club/img/71/9616090/25272530.jpg','https://musify.club/release/juno-reactor-pistolero-1999-8964'),
 (9416,30,'The Ring / The Spiral - OST / Звонок / Спираль - Саундтрек','2016-01-01 00:00:00',0,'https://38s.musify.club/img/69/10556272/27417881.jpg','https://musify.club/release/the-ring-the-spiral-ost-zvonok-spiral-saundtrek-1998-675825'),
 (9417,30,'Enigma-Forest-Transit 8','2017-07-09 00:00:00',0,'https://41s.musify.club/img/68/14126203/36155712.jpg','https://musify.club/release/enigma-forest-transit-8-1998-872757'),
 (9418,30,'Dance &amp; Levi&#39;s 98','2019-09-09 00:00:00',0,'https://40s-a.musify.club/img/71/19524842/50507606.jpg','https://musify.club/release/dance-and-levis-98-1998-1196059'),
 (9419,30,'Alien Software for Alien Hardware','2015-09-18 00:00:00',0,'https://39s.musify.club/img/69/10059817/26250494.jpg','https://musify.club/release/alien-software-for-alien-hardware-1998-648216'),
 (9420,30,'Retrodelica 2 - Back 2 the Future','2015-02-27 00:00:00',0,'https://37s.musify.club/img/69/9032667/24116631.jpg','https://musify.club/release/retrodelica-2-back-2-the-future-1998-601050'),
 (9421,30,'Rave Around The World (The Best Of Vox&#39;S Night Loop) [CD1]','2010-07-22 00:00:00',0,'https://41s-a.musify.club/img/70/1129692/40260889.jpg','https://musify.club/release/rave-around-the-world-the-best-of-voxs-night-loop-cd1-1998-204524'),
 (9422,30,'Lost In Space - OST / Затерянные В Космосе - Саундтрек','2014-08-03 00:00:00',0,'https://40s.musify.club/img/69/7731433/21507393.jpg','https://musify.club/release/lost-in-space-ost-zateryannie-v-kosmose-saundtrek-1998-525621'),
 (9423,30,'This Is... Goa Vol. 1 - The Best Of Goa-Head [CD1]','2022-04-03 00:00:00',0,'https://41s.musify.club/img/68/25466586/64042613.jpg','https://musify.club/release/this-is-goa-vol-1-the-best-of-goa-head-cd1-1998-1606328'),
 (9424,30,'This Is... Goa Volume 1 (The Best Of Tantrance) [CD1]','2021-05-09 00:00:00',0,'https://38s.musify.club/img/68/23826406/60552661.jpg','https://musify.club/release/this-is-goa-volume-1-the-best-of-tantrance-cd1-1998-1490124'),
 (9425,30,'In To The Mix [CD1]','2021-11-12 00:00:00',0,'https://40s-a.musify.club/img/70/24437831/62585656.jpg','https://musify.club/release/in-to-the-mix-cd1-1997-1552411'),
 (9426,30,'God Is God (Front 242 Mixes)','2015-06-09 00:00:00',0,'https://37s.musify.club/img/69/9600892/25293945.jpg','https://musify.club/release/juno-reactor-god-is-god-front-242-mixes-1997-625537'),
 (9427,30,'Mind The Gap Volume 16','2020-01-04 00:00:00',0,'https://39s.musify.club/img/69/20207841/51971415.jpg','https://musify.club/release/mind-the-gap-volume-16-1997-1237453'),
 (9428,30,'Psychedelic Vibes 2','2015-04-12 00:00:00',0,'https://37s.musify.club/img/69/9281856/24612056.jpg','https://musify.club/release/psychedelic-vibes-2-1997-612636'),
 (9429,30,'Retrodelica - Back from the Future','2015-02-27 00:00:00',0,'https://37s.musify.club/img/69/9032653/24116625.jpg','https://musify.club/release/retrodelica-back-from-the-future-1997-601060'),
 (9430,30,'The Perfecto Allstars Present: The Perfect Party','2020-05-29 00:00:00',0,'https://38s.musify.club/img/69/21216594/54096017.jpg','https://musify.club/release/the-perfecto-allstars-present-the-perfect-party-1997-1295511'),
 (9431,30,'Conga Fury','2008-08-28 00:00:00',0,'https://37s.musify.club/img/69/9600785/25238027.jpg','https://musify.club/release/juno-reactor-conga-fury-1997-19999'),
 (9432,30,'Mortal Kombat: Annihilation (Promo Sampler)','2011-06-22 00:00:00',0,'https://41s-a.musify.club/img/70/1192044/40197281.jpg','https://musify.club/release/mortal-kombat-annihilation-promo-sampler-1997-225273'),
 (9433,30,'Tantrance 5 - A Trip to Psychedelic Trance','2008-11-01 00:00:00',0,'https://37s.musify.club/img/69/9810126/25679940.jpg','https://musify.club/release/tantrance-5-a-trip-to-psychedelic-trance-1997-201427'),
 (9434,30,'Big Rock&#39;n Beats','2014-03-15 00:00:00',0,'https://38s-a.musify.club/img/70/6850203/19743514.jpg','https://musify.club/release/big-rockn-beats-1997-481922'),
 (9435,30,'Mortal Kombat 2: Аnnihilation - OST / Смертельная битва 2: Истребление - Саундтрек','2007-01-14 00:00:00',0,'https://39s.musify.club/img/69/4630323/15346946.jpg','https://musify.club/release/mortal-kombat-2-annihilation-ost-smertelnaya-bitva-2-istreblenie-saund-1997-200103'),
 (9436,30,'Wing Commander: Prophecy - OST','2013-09-11 00:00:00',0,'https://40s.musify.club/img/69/5185078/17050639.jpg','https://musify.club/release/wing-commander-prophecy-ost-1997-398858'),
 (9437,30,'Bible Of Dreams','2008-06-15 00:00:00',0,'https://41s-a.musify.club/img/70/780932/40611356.jpg','https://musify.club/release/juno-reactor-bible-of-dreams-1997-16762'),
 (9438,30,'Jungle High','2008-08-28 00:00:00',0,'https://39s.musify.club/img/69/9600827/25238138.jpg','https://musify.club/release/juno-reactor-jungle-high-1997-20076'),
 (9439,30,'The History Of Trance. Part 2: &#39;91-&#39;96 (CD1)','2021-11-12 00:00:00',0,'https://38s.musify.club/img/68/24566187/62586356.jpg','https://musify.club/release/the-history-of-trance-part-2-91-96-cd1-1997-1552439'),
 (9440,30,'The History Of Trance. Part 3: The Psychedelic Movement (CD1)','2021-11-12 00:00:00',0,'https://38s.musify.club/img/68/24571547/62586445.jpg','https://musify.club/release/the-history-of-trance-part-3-the-psychedelic-movement-cd1-1997-1552443'),
 (9441,30,'God Is God','2008-08-27 00:00:00',0,'https://41s-a.musify.club/img/70/790232/40602067.jpg','https://musify.club/release/juno-reactor-god-is-god-1997-19863'),
 (9442,30,'Tantrance 2 - A Trip to Psychedelic Trance','2015-07-12 00:00:00',0,'https://39s-a.musify.club/img/71/9770419/25611902.jpg','https://musify.club/release/tantrance-2-a-trip-to-psychedelic-trance-1996-633270'),
 (9443,30,'Samurai','2008-06-15 00:00:00',0,'https://41s-a.musify.club/img/70/780929/40611359.jpg','https://musify.club/release/juno-reactor-samurai-1996-16761'),
 (9444,30,'Distance To Goa 3 - 2CD [CD1]','2015-03-05 00:00:00',0,'https://39s.musify.club/img/68/9068483/24192340.jpg','https://musify.club/release/distance-to-goa-3-2cd-cd1-1996-602863'),
 (9445,30,'Hypnotic Sounds: 17 Trips In Trance','2019-11-02 00:00:00',0,'https://38s.musify.club/img/69/19897815/51313101.jpg','https://musify.club/release/hypnotic-sounds-17-trips-in-trance-1996-1220914'),
 (9446,30,'Mortal Kombat: More Kombat','2008-03-13 00:00:00',0,'https://39s-a.musify.club/img/70/6455308/18951585.jpg','https://musify.club/release/mortal-kombat-more-kombat-1996-200644'),
 (9447,30,'Goa-Head Vol.1 CD1','2011-09-24 00:00:00',0,'https://41s-a.musify.club/img/70/1255968/40132090.jpg','https://musify.club/release/goa-head-vol-1-cd1-1996-246583'),
 (9448,30,'Sound of GAiA Party','2015-06-22 00:00:00',0,'https://39s.musify.club/img/68/9659204/25429724.jpg','https://musify.club/release/sound-of-gaia-party-1995-628327'),
 (9449,30,'Guardian Angel','2008-08-28 00:00:00',0,'https://41s-a.musify.club/img/70/790520/40601780.jpg','https://musify.club/release/juno-reactor-guardian-angel-1995-19959'),
 (9450,30,'Beyond the Infinite','2008-06-15 00:00:00',0,'https://41s-a.musify.club/img/70/780923/40611365.jpg','https://musify.club/release/juno-reactor-beyond-the-infinite-1995-16759'),
 (9451,30,'Virtuosity - OST / Виртуозность - Саундтрек','2010-03-19 00:00:00',0,'https://41s-a.musify.club/img/70/1125885/40264685.jpg','https://musify.club/release/virtuosity-ost-virtuoznost-saundtrek-1995-203257'),
 (9452,30,'Tantrance - A Trip to Psychedelic Trance','2015-07-12 00:00:00',0,'https://39s-a.musify.club/img/71/9769741/25611725.jpg','https://musify.club/release/tantrance-a-trip-to-psychedelic-trance-1995-633268'),
 (9453,30,'Dream Injection Vol. 1','2016-12-16 00:00:00',0,'https://41s.musify.club/img/69/13554676/34957730.jpg','https://musify.club/release/dream-injection-vol-1-1995-793120'),
 (9454,30,'High Energy Protons','2008-08-28 00:00:00',0,'https://41s-a.musify.club/img/70/790451/40601848.jpg','https://musify.club/release/juno-reactor-high-energy-protons-1994-19936'),
 (9455,30,'Luciana','2009-11-09 00:00:00',0,'https://41s-a.musify.club/img/70/881201/40510421.jpg','https://musify.club/release/juno-reactor-luciana-1994-50191'),
 (9456,30,'Laughing Gas','2008-08-27 00:00:00',0,'https://41s-a.musify.club/img/70/790337/40601962.jpg','https://musify.club/release/juno-reactor-laughing-gas-1994-19898'),
 (9457,30,'Volume Eight','2015-02-04 00:00:00',0,'https://39s.musify.club/img/68/8903111/23851685.jpg','https://musify.club/release/volume-eight-1993-594678'),
 (9458,30,'Transmissions','2008-01-17 00:00:00',0,'https://39s-a.musify.club/img/71/9554895/25148298.jpg','https://musify.club/release/juno-reactor-transmissions-1993-8771'),
 (9459,30,'Volume Compilations Vol.8','2020-12-23 00:00:00',0,'https://38s.musify.club/img/68/22827007/58433959.jpg','https://musify.club/release/volume-compilations-vol-8-1993-1418206'),
 (9460,30,'Charcoal Collezione Deluxe Editione','2012-02-02 00:00:00',0,'https://39s.musify.club/img/69/3138373/12970470.jpg','https://musify.club/release/charcoal-collezione-deluxe-editione-280414'),
 (9461,29,'Progressive Psychedelic Trance 2022','2022-01-22 00:00:00',0,'https://37s-a.musify.club/img/71/24985429/63036700.jpg','https://musify.club/release/progressive-psychedelic-trance-2022-2021-1568806'),
 (9462,29,'Beatport Psy Trance. Sound Pack #321','2021-10-06 00:00:00',0,'https://37s.musify.club/img/69/24537813/62308584.jpg','https://musify.club/release/beatport-psy-trance-sound-pack-321-2021-1544994'),
 (9463,29,'Progressive Addiction, vol. 1','2020-04-01 00:00:00',0,'https://39s-a.musify.club/img/70/20783086/53146336.jpg','https://musify.club/release/progressive-addiction-vol-1-2020-1269846'),
 (9464,29,'Workout Music Electronica Top 100 Best Selling Chart Hits','2020-09-14 00:00:00',0,'https://40s.musify.club/img/68/22040644/57309962.jpg','https://musify.club/release/workout-music-electronica-top-100-best-selling-chart-hits-2020-1375018'),
 (9465,29,'Beatport Psy Trance. Electro Sound Pack #197','2020-11-28 00:00:00',0,'https://40s.musify.club/img/68/22637886/58042501.jpg','https://musify.club/release/beatport-psy-trance-electro-sound-pack-197-2020-1406169'),
 (9466,29,'Progressive Goa Rave 2020 Top 100 Hits DJ Mix','2020-09-10 00:00:00',0,'https://39s.musify.club/img/68/22012288/56539118.jpg','https://musify.club/release/progressive-goa-rave-2020-top-100-hits-dj-mix-2020-1373565'),
 (9467,29,'Progressive Goa Trance 2020 Top 100 Hits DJ Mix','2020-09-10 00:00:00',0,'https://39s.musify.club/img/68/22012292/56539125.jpg','https://musify.club/release/progressive-goa-trance-2020-top-100-hits-dj-mix-2020-1373573'),
 (9468,29,'Progressive Psy Trance 2020 100 Vibes DJ Mix','2020-09-10 00:00:00',0,'https://39s.musify.club/img/68/22012305/56539147.jpg','https://musify.club/release/progressive-psy-trance-2020-100-vibes-dj-mix-2020-1373576'),
 (9469,29,'Progressive Goa Trance Beach 2020 100 Vibes DJ Mix','2020-09-10 00:00:00',0,'https://39s.musify.club/img/68/22012296/56539132.jpg','https://musify.club/release/progressive-goa-trance-beach-2020-100-vibes-dj-mix-2020-1373574'),
 (9470,29,'Beatport Psychedelic Trance. Electro Sound Pack #44 CD1','2020-04-08 00:00:00',0,'https://38s.musify.club/img/69/20817108/53244503.jpg','https://musify.club/release/beatport-psychedelic-trance-electro-sound-pack-44-cd1-2020-1272673'),
 (9471,29,'Progressive House Techno Top 100 Hits DJ Mix','2020-09-10 00:00:00',0,'https://39s.musify.club/img/68/22012302/56539141.jpg','https://musify.club/release/progressive-house-techno-top-100-hits-dj-mix-2020-1373575'),
 (9472,29,'Heart of Goa, v.7','2020-11-14 00:00:00',0,'https://38s.musify.club/img/68/22537783/57841256.jpg','https://musify.club/release/heart-of-goa-v-7-2020-1400693'),
 (9473,29,'GoaTrance PsyStoned, vol. 10','2021-01-06 00:00:00',0,'https://38s.musify.club/img/68/22929567/58657029.jpg','https://musify.club/release/goatrance-psystoned-vol-10-2020-1424519'),
 (9474,29,'Psychedelic Progressive Trance 2020, v.4','2020-06-01 00:00:00',0,'https://39s-a.musify.club/img/71/21237977/54145919.jpg','https://musify.club/release/psychedelic-progressive-trance-2020-v-4-2020-1296719'),
 (9475,29,'Beatport Progressive Trance. Electro Sound Pack #89','2020-06-02 00:00:00',0,'https://37s-a.musify.club/img/68/21257777/54162578.jpg','https://musify.club/release/beatport-progressive-trance-electro-sound-pack-89-2020-1297231'),
 (9476,29,'Progressive Psychedelic Trance Spotlight, vol. 1','2020-02-15 00:00:00',0,'https://40s-a.musify.club/img/71/20471136/52519540.jpg','https://musify.club/release/progressive-psychedelic-trance-spotlight-vol-1-2020-1253706'),
 (9477,29,'Neurochemistry. Psychedelic Trance 2020','2020-02-14 00:00:00',0,'https://39s-a.musify.club/img/71/20461800/52500423.jpg','https://musify.club/release/neurochemistry-psychedelic-trance-2020-2020-1252953'),
 (9478,29,'Portal Transform. Psy Trance Music 2020','2020-02-14 00:00:00',0,'https://40s-a.musify.club/img/71/20463494/52501452.jpg','https://musify.club/release/portal-transform-psy-trance-music-2020-2020-1252992'),
 (9479,29,'Dance 2019','2019-04-17 00:00:00',0,'https://38s-a.musify.club/img/71/18588385/48581783.jpg','https://musify.club/release/dance-2019-2019-1133245'),
 (9480,29,'Somatic Cell. Hyperactive Psy Trance (2019)','2019-10-25 00:00:00',0,'https://38s.musify.club/img/69/19851015/51201873.jpg','https://musify.club/release/somatic-cell-hyperactive-psy-trance-2019-2019-1217889'),
 (9481,29,'Wilderness Sunsonic Psy Trance','2020-03-16 00:00:00',0,'https://38s.musify.club/img/69/20638104/52893913.jpg','https://musify.club/release/wilderness-sunsonic-psy-trance-2019-1262770'),
 (9482,29,'Goa Moon, vol. 10','2019-06-17 00:00:00',0,'https://40s-a.musify.club/img/71/18990528/49360598.jpg','https://musify.club/release/goa-moon-vol-10-2019-1158644'),
 (9483,29,'GoaTrance PsyStoned, Vol. 1','2019-03-14 00:00:00',0,'https://40s.musify.club/img/69/18349951/48077491.jpg','https://musify.club/release/goatrance-psystoned-vol-1-2019-1117598'),
 (9484,29,'Abstrct Vision: Psychedelic Trance','2019-12-06 00:00:00',0,'https://39s-a.musify.club/img/70/20092714/51705468.jpg','https://musify.club/release/abstrct-vision-psychedelic-trance-2019-1231591'),
 (9485,29,'Progressive Fullon Light 2019','2019-08-02 00:00:00',0,'https://40s-a.musify.club/img/71/19286336/49990347.jpg','https://musify.club/release/progressive-fullon-light-2019-2019-1180369'),
 (9486,29,'GoaTrance PsyStoned, vol. 2','2019-05-06 00:00:00',0,'https://40s-a.musify.club/img/71/18720854/48770895.jpg','https://musify.club/release/goatrance-psystoned-vol-2-2019-1140635'),
 (9487,29,'EDM Rave Dance Music Explosion. Top 100 Massive Festival Hits','2020-03-03 00:00:00',0,'https://39s.musify.club/img/69/20659118/52866940.jpg','https://musify.club/release/edm-rave-dance-music-explosion-top-100-massive-festival-hits-2019-1257469'),
 (9488,29,'EDM 2019','2019-03-27 00:00:00',0,'https://40s-a.musify.club/img/70/18450249/48261352.jpg','https://musify.club/release/edm-2019-2019-1123867'),
 (9489,29,'Psychedelic Trance Party','2019-07-23 00:00:00',0,'https://40s-a.musify.club/img/71/19208683/49859993.jpg','https://musify.club/release/psychedelic-trance-party-2019-1176690'),
 (9490,29,'Progressive Goa Trance 2019, vol. 2','2019-06-09 00:00:00',0,'https://40s-a.musify.club/img/71/18938051/49255284.jpg','https://musify.club/release/progressive-goa-trance-2019-vol-2-2019-1155317'),
 (9491,29,'The Gold Hornet','2019-06-28 00:00:00',0,'https://40s-a.musify.club/img/71/19055583/49500223.jpg','https://musify.club/release/elepho-the-gold-hornet-2019-1163341'),
 (9492,29,'Autumn Selection by Ascent and Mina','2019-10-29 00:00:00',0,'https://38s.musify.club/img/69/19886439/51265340.jpg','https://musify.club/release/autumn-selection-by-ascent-and-mina-2019-1219410'),
 (9493,29,'Kontakt','2019-05-31 00:00:00',0,'https://40s-a.musify.club/img/71/18895185/49145705.jpg','https://musify.club/release/elepho-kontakt-2019-1152406'),
 (9494,29,'Transition','2019-04-12 00:00:00',0,'https://38s-a.musify.club/img/71/18551125/48533542.jpg','https://musify.club/release/elepho-transition-2019-1130809'),
 (9495,29,'Electronic 2019','2019-04-17 00:00:00',0,'https://40s-a.musify.club/img/71/18589493/48583125.jpg','https://musify.club/release/electronic-2019-2019-1133289'),
 (9496,29,'Techno Trance 2018 100 Hits DJ Mix','2020-07-21 00:00:00',0,'https://38s.musify.club/img/69/21628482/54896107.jpg','https://musify.club/release/techno-trance-2018-100-hits-dj-mix-2018-1319971'),
 (9497,29,'Progressive Goa Trance 2018 Top 100 Hits DJ Mix Part 1','2020-09-11 00:00:00',0,'https://39s.musify.club/img/68/22014747/56548612.jpg','https://musify.club/release/progressive-goa-trance-2018-top-100-hits-dj-mix-part-1-2018-1373986'),
 (9498,29,'Psy Trance Euphoria','2020-03-21 00:00:00',0,'https://40s-a.musify.club/img/71/20791857/53152180.jpg','https://musify.club/release/psy-trance-euphoria-2018-1264852'),
 (9499,29,'Goa Psy Trance 2018 Top 100 Hits DJ Mix','2020-09-11 00:00:00',0,'https://39s.musify.club/img/68/22014725/56548575.jpg','https://musify.club/release/goa-psy-trance-2018-top-100-hits-dj-mix-2018-1373984'),
 (9500,29,'Techno House Trance 2018 Top 100 Hits DJ Mix','2020-09-14 00:00:00',0,'https://40s.musify.club/img/68/22055188/57327727.jpg','https://musify.club/release/techno-house-trance-2018-top-100-hits-dj-mix-2018-1375072'),
 (9501,29,'Cybertronic. Trance Psychedelic','2019-02-15 00:00:00',0,'https://39s-a.musify.club/img/70/18151847/47699386.jpg','https://musify.club/release/cybertronic-trance-psychedelic-2018-1105778'),
 (9502,29,'Psychedelic Progressive Goa Trance Top 100 Best Selling Chart Hits V2','2020-09-11 00:00:00',0,'https://39s.musify.club/img/68/22009896/56536140.jpg','https://musify.club/release/psychedelic-progressive-goa-trance-top-100-best-selling-chart-hits-v2-2018-1373974'),
 (9503,29,'Progressive Goa Trance 2018 100 Hits DJ Mix','2020-07-21 00:00:00',0,'https://38s.musify.club/img/69/21628296/54895972.jpg','https://musify.club/release/progressive-goa-trance-2018-100-hits-dj-mix-2018-1319970'),
 (9504,29,'Milky Way','2018-02-07 00:00:00',0,'https://41s.musify.club/img/68/15646205/40758964.jpg','https://musify.club/release/elepho-milky-way-2018-959986'),
 (9505,29,'Reconnaissance','2019-01-10 00:00:00',0,'https://39s-a.musify.club/img/70/17886474/47108315.jpg','https://musify.club/release/elepho-reconnaissance-2018-1090379'),
 (9506,29,'Party Warriors: Progressive &amp; Psychedelic Goa Trance','2018-04-04 00:00:00',0,'https://41s-a.musify.club/img/71/15989037/42777728.jpg','https://musify.club/release/party-warriors-progressive-and-psychedelic-goa-trance-2018-979162'),
 (9507,29,'Progressive Goa Trance 2018 Top 100 Hits DJ Mix Part 2','2020-09-11 00:00:00',0,'https://38s.musify.club/img/68/22099903/57356675.jpg','https://musify.club/release/progressive-goa-trance-2018-top-100-hits-dj-mix-part-2-2018-1374002'),
 (9508,29,'EDM Rave Dance Party 2018 100 Hits DJ Mix','2020-07-21 00:00:00',0,'https://38s.musify.club/img/69/21628776/54896395.jpg','https://musify.club/release/edm-rave-dance-party-2018-100-hits-dj-mix-2018-1319983'),
 (9509,29,'Progressive Trance 2019','2018-12-22 00:00:00',0,'https://37s.musify.club/img/69/17774210/46952480.jpg','https://musify.club/release/progressive-trance-2019-2018-1082018'),
 (9510,29,'Dark Shades','2019-02-23 00:00:00',0,'https://39s-a.musify.club/img/70/18223710/47794812.jpg','https://musify.club/release/dark-shades-2018-1110048'),
 (9511,29,'Escape','2018-11-25 00:00:00',0,'https://37s-a.musify.club/img/71/17591144/46309705.jpg','https://musify.club/release/elepho-escape-2018-1070125'),
 (9512,29,'Chronicles','2018-11-25 00:00:00',0,'https://37s-a.musify.club/img/71/17591164/46309714.jpg','https://musify.club/release/elepho-chronicles-2018-1070126'),
 (9513,29,'Progressive Psychodelic Trance Vol.4 (Exlusive Tracks)','2019-02-18 00:00:00',0,'https://39s.musify.club/img/69/18177926/47732379.jpg','https://musify.club/release/progressive-psychodelic-trance-vol-4-exlusive-tracks-2018-1107590'),
 (9514,29,'Progressive Trance &amp; Groovy Tech-House Top 100 Best Selling Chart Hits + DJ Mix','2020-09-11 00:00:00',0,'https://39s.musify.club/img/68/22023994/56557922.jpg','https://musify.club/release/progressive-trance-and-groovy-tech-house-top-100-best-selling-chart-hi-2017-1374011'),
 (9515,29,'Workout Music 2017 Top 100 Hits Techno Trance House Dubstep EDM Fitness 8 Hr DJ Mix','2020-08-31 00:00:00',0,'https://39s.musify.club/img/68/21906710/56082141.jpg','https://musify.club/release/workout-music-2017-top-100-hits-techno-trance-house-dubstep-edm-fitnes-2017-1363399'),
 (9516,29,'Running Trance Workout Music 2017 Top 100 Hits 8 Hr DJ Mix','2020-08-31 00:00:00',0,'https://39s.musify.club/img/68/21908712/56084331.jpg','https://musify.club/release/running-trance-workout-music-2017-top-100-hits-8-hr-dj-mix-2017-1363400'),
 (9517,29,'Progressive Psychedelic Trance Top 100','2020-04-08 00:00:00',0,'https://39s-a.musify.club/img/70/20823447/53253820.jpg','https://musify.club/release/progressive-psychedelic-trance-top-100-2017-1273023'),
 (9518,29,'Trance Top 100 Best Selling Chart Hits','2020-06-09 00:00:00',0,'https://38s.musify.club/img/68/21310227/54291965.jpg','https://musify.club/release/trance-top-100-best-selling-chart-hits-2017-1300727'),
 (9519,29,'Hard Dance 2017 Top 100','2017-09-13 00:00:00',0,'https://41s-a.musify.club/img/70/14622405/37231756.jpg','https://musify.club/release/hard-dance-2017-top-100-2017-905063'),
 (9520,29,'Summer Sun 2','2018-04-21 00:00:00',0,'https://41s-a.musify.club/img/70/16113990/43035967.jpg','https://musify.club/release/summer-sun-2-2017-986846'),
 (9521,29,'Progressive Goa Trance Beach 2017 Top 100 Hits DJ Mix','2020-09-12 00:00:00',0,'https://39s.musify.club/img/68/22007230/56533196.jpg','https://musify.club/release/progressive-goa-trance-beach-2017-top-100-hits-dj-mix-2017-1374461'),
 (9522,29,'Techno Tech House 2017 Top 100 Hits DJ Mix','2020-09-11 00:00:00',0,'https://39s.musify.club/img/68/22014772/56548651.jpg','https://musify.club/release/techno-tech-house-2017-top-100-hits-dj-mix-2017-1373988'),
 (9523,29,'Workout Music 2016 100 Top Trance Hits + 1 Hr DJ Mix','2020-08-31 00:00:00',0,'https://39s.musify.club/img/68/21906704/56082135.jpg','https://musify.club/release/workout-music-2016-100-top-trance-hits-1-hr-dj-mix-2016-1363354'),
 (9524,29,'Psychedelic Trance 2017 Top 100 Hits DJ Mix','2020-07-24 00:00:00',0,'https://39s-a.musify.club/img/70/21627997/54965415.jpg','https://musify.club/release/psychedelic-trance-2017-top-100-hits-dj-mix-2016-1321967'),
 (9525,29,'Psy Trance Treasures 2016 - 100 Best Of Top Full-On Progressive &amp; Psychedelic Goa Hits','2020-09-12 00:00:00',0,'https://39s.musify.club/img/68/22012313/56539161.jpg','https://musify.club/release/psy-trance-treasures-2016-100-best-of-top-full-on-progressive-and-psyc-2016-1374488'),
 (9526,29,'Progressive Goa Trance 2017 Top 100 Hits DJ Mix','2020-07-24 00:00:00',0,'https://39s-a.musify.club/img/70/21629812/54965522.jpg','https://musify.club/release/progressive-goa-trance-2017-top-100-hits-dj-mix-2016-1321968'),
 (9527,29,'100 Top Trance Workout Power Hits + 1 Hour DJ Mix 2015','2020-09-15 00:00:00',0,'https://40s.musify.club/img/68/22051234/57323710.jpg','https://musify.club/release/100-top-trance-workout-power-hits-1-hour-dj-mix-2015-2016-1375509'),
 (9528,29,'Progressive Goa 2017 - Best Of Top 100','2016-12-15 00:00:00',0,'https://41s.musify.club/img/69/12635016/32812131.jpg','https://musify.club/release/progressive-goa-2017-best-of-top-100-2016-792593'),
 (9529,29,'Global Trance Grooves 159 (14-06-2016) Jeremy Rowlett Guestmix','2020-01-27 00:00:00',0,'https://39s-a.musify.club/img/70/20311687/52180261.jpg','https://musify.club/release/global-trance-grooves-159-14-06-2016-jeremy-rowlett-guestmix-2016-1241532'),
 (9530,29,'Goa 2017: 30 Top Best Of Hits Progressive Psytrance &amp; Psychedelic Electronic Dance','2016-12-09 00:00:00',0,'https://41s.musify.club/img/68/12588246/32722573.jpg','https://musify.club/release/goa-2017-30-top-best-of-hits-progressive-psytrance-and-psychedelic-ele-2016-790151'),
 (9531,29,'Goa Journeys 2010-2012','2019-01-12 00:00:00',0,'https://39s-a.musify.club/img/70/17886539/47108425.jpg','https://musify.club/release/elepho-goa-journeys-2010-2012-2016-1091064'),
 (9532,29,'Deep House 2017 Top 100 Hits DJ Mix','2020-09-15 00:00:00',0,'https://40s.musify.club/img/68/22055183/57327721.jpg','https://musify.club/release/deep-house-2017-top-100-hits-dj-mix-2016-1375513'),
 (9533,29,'EDM Workout Music Hits 2016 - Top 100 Trance + Dubstep Remixes 6 Hrs','2020-09-01 00:00:00',0,'https://39s.musify.club/img/68/21911882/56087945.jpg','https://musify.club/release/edm-workout-music-hits-2016-top-100-trance-dubstep-remixes-6-hrs-2016-1363881'),
 (9534,29,'Chill Out Ambient Lounge 2016 (Top 100 Hits + 4Hr DJ Mix)','2020-09-11 00:00:00',0,'https://39s.musify.club/img/68/22012272/56539066.jpg','https://musify.club/release/chill-out-ambient-lounge-2016-top-100-hits-4hr-dj-mix-2016-1373981'),
 (9535,29,'Progressive Goa Trance 2016 Vol. 2','2016-07-18 00:00:00',0,'https://38s-a.musify.club/img/70/11750974/30928532.jpg','https://musify.club/release/progressive-goa-trance-2016-vol-2-2016-744347'),
 (9536,29,'Progressive Psychedelic Goa Trance Top 100 Best Selling Chart Hits + DJ Mix','2020-09-13 00:00:00',0,'https://39s.musify.club/img/68/22023990/56557915.jpg','https://musify.club/release/progressive-psychedelic-goa-trance-top-100-best-selling-chart-hits-dj--2016-1374549'),
 (9537,29,'100 Top Super Psychedelic &amp; Progressive Trance Hits V2','2020-09-12 00:00:00',0,'https://39s.musify.club/img/68/22012261/56539046.jpg','https://musify.club/release/100-top-super-psychedelic-and-progressive-trance-hits-v2-2015-1374486'),
 (9538,29,'101 Progressive Trance Dance Hits DJ Mix 2015','2020-09-12 00:00:00',0,'https://39s.musify.club/img/68/22012264/56539052.jpg','https://musify.club/release/101-progressive-trance-dance-hits-dj-mix-2015-2015-1374487'),
 (9539,29,'Road','2015-08-13 00:00:00',0,'https://39s.musify.club/img/68/9915134/25922934.jpg','https://musify.club/release/elepho-road-2015-641069'),
 (9540,29,'Do You Know','2018-12-30 00:00:00',0,'https://40s.musify.club/img/69/17798326/46985450.jpg','https://musify.club/release/elepho-do-you-know-2015-1085622'),
 (9541,29,'99 Progressive, Fullon &amp; Goa Psytrance Festival Rave Hits','2020-07-24 00:00:00',0,'https://40s.musify.club/img/68/21627182/54966119.jpg','https://musify.club/release/99-progressive-fullon-and-goa-psytrance-festival-rave-hits-2014-1321980'),
 (9542,29,'House Of Trance Top 100 Trance Hits 2014 - Electronic Dance Music Night Club Electronica Disco Tech DJ Mix Essentials','2020-09-08 00:00:00',0,'https://39s.musify.club/img/68/21996202/56440938.jpg','https://musify.club/release/house-of-trance-top-100-trance-hits-2014-electronic-dance-music-night--2014-1372928'),
 (9543,29,'Workout Music Dubstep Trance House 101 Hits 2014','2020-09-01 00:00:00',0,'https://39s.musify.club/img/68/21911897/56087981.jpg','https://musify.club/release/workout-music-dubstep-trance-house-101-hits-2014-2014-1363886'),
 (9544,29,'Hardstyle Dance Top 100 Hits 2014','2020-08-31 00:00:00',0,'https://39s.musify.club/img/68/21904439/56080239.jpg','https://musify.club/release/hardstyle-dance-top-100-hits-2014-2014-1363123'),
 (9545,29,'New Vision','2014-06-15 00:00:00',0,'https://40s.musify.club/img/69/7481242/20997079.jpg','https://musify.club/release/new-vision-2014-513075'),
 (9546,29,'Deeper','2019-06-28 00:00:00',0,'https://40s-a.musify.club/img/71/19056043/49500839.jpg','https://musify.club/release/elepho-deeper-2014-1163366'),
 (9547,29,'Progressive Goa Trance Vol.1 (CD2)','2014-05-03 00:00:00',0,'https://38s.musify.club/img/69/7198169/20416931.jpg','https://musify.club/release/progressive-goa-trance-vol-1-cd2-2014-498654'),
 (9548,29,'Paranormal Activity','2014-02-12 00:00:00',0,'https://38s-a.musify.club/img/70/6694977/19412615.jpg','https://musify.club/release/elepho-paranormal-activity-2014-472614'),
 (9549,29,'Dubstep Ukf 100 Hits 2014','2020-09-01 00:00:00',0,'https://39s.musify.club/img/68/21911870/56087921.jpg','https://musify.club/release/dubstep-ukf-100-hits-2014-2014-1363850'),
 (9550,29,'Go-Addiction','2019-06-28 00:00:00',0,'https://40s-a.musify.club/img/71/19055867/49500763.jpg','https://musify.club/release/go-addiction-2014-1163358'),
 (9551,29,'Landscape','2019-06-28 00:00:00',0,'https://40s-a.musify.club/img/71/19055988/49500813.jpg','https://musify.club/release/elepho-landscape-2014-1163363'),
 (9552,29,'Renewal','2019-06-28 00:00:00',0,'https://40s-a.musify.club/img/71/19055941/49500797.jpg','https://musify.club/release/elepho-renewal-2014-1163360'),
 (9553,29,'Progressive Goa Trance 2014 Vol. 3','2014-08-08 00:00:00',0,'https://38s-a.musify.club/img/70/7780684/21550898.jpg','https://musify.club/release/progressive-goa-trance-2014-vol-3-2014-526673'),
 (9554,29,'Top 100 Downtempo Ambient &amp; Chillout Lounge Meditational Relaxing Grooves - Best Selling Chart Hits 2014 + 1Hr DJ Mix','2020-09-08 00:00:00',0,'https://39s.musify.club/img/68/21999536/56444554.jpg','https://musify.club/release/top-100-downtempo-ambient-and-chillout-lounge-meditational-relaxing-gr-2014-1372956'),
 (9555,29,'Rave 100 Rave Club Hits 2014','2020-08-31 00:00:00',0,'https://39s.musify.club/img/68/21904464/56080260.jpg','https://musify.club/release/rave-100-rave-club-hits-2014-2014-1363126'),
 (9556,29,'Tech House 100 Tech House Hits','2020-09-08 00:00:00',0,'https://39s.musify.club/img/68/21996214/56440956.jpg','https://musify.club/release/tech-house-100-tech-house-hits-2014-1372931'),
 (9557,29,'Electro Techno 100 Hits 2014','2020-09-08 00:00:00',0,'https://39s.musify.club/img/68/21996199/56440932.jpg','https://musify.club/release/electro-techno-100-hits-2014-2014-1372927'),
 (9558,29,'Workout Music Top 100 Hits 2014 + 8 One Hour DJ Mix','2020-09-15 00:00:00',0,'https://39s.musify.club/img/68/22014777/56548659.jpg','https://musify.club/release/workout-music-top-100-hits-2014-8-one-hour-dj-mix-2014-1375493'),
 (9559,29,'101 Techno Hits 2013 - Best Of Top Acid Techno Trance Psy Nrg Electro House Tech House Goa Psychedelic Rave Anthems','2020-09-08 00:00:00',0,'https://39s.musify.club/img/68/21996190/56440920.jpg','https://musify.club/release/101-techno-hits-2013-best-of-top-acid-techno-trance-psy-nrg-electro-ho-2013-1372926'),
 (9560,29,'7H30','2019-06-28 00:00:00',0,'https://40s-a.musify.club/img/71/19056091/49500939.jpg','https://musify.club/release/elepho-7h30-2013-1163370'),
 (9561,29,'Essenciel','2019-06-28 00:00:00',0,'https://40s-a.musify.club/img/71/19056056/49500855.jpg','https://musify.club/release/elepho-essenciel-2013-1163368'),
 (9562,29,'99 Best Of Club Hits 2013 - Top EDM Rave Psytrance Electro Techno','2020-09-23 00:00:00',0,'https://38s.musify.club/img/68/22154628/57410408.jpg','https://musify.club/release/99-best-of-club-hits-2013-top-edm-rave-psytrance-electro-techno-2013-1378833'),
 (9563,29,'Progressive Goa 2013 - Best Of Top 100 Electronic Dance Techno House Psytrance Festival Party','2020-09-08 00:00:00',0,'https://39s.musify.club/img/68/21996208/56440944.jpg','https://musify.club/release/progressive-goa-2013-best-of-top-100-electronic-dance-techno-house-psy-2013-1372929'),
 (9564,28,'Floating Pyramids: Ambient Meditation PT.1','2022-04-17 00:00:00',0,'https://37s-a.musify.club/img/71/25596893/64231008.jpg','https://musify.club/release/floating-pyramids-ambient-meditation-pt-1-2022-1611404'),
 (9565,28,'The Atmosphere Harmony: Ambient Antistress Collection PT.1','2022-05-05 00:00:00',0,'https://40s-a.musify.club/img/70/25725463/64456099.jpg','https://musify.club/release/the-atmosphere-harmony-ambient-antistress-collection-pt-1-2022-1617103'),
 (9566,28,'Ambient Space Sounds','2022-06-22 00:00:00',0,'https://39s.musify.club/img/68/25958615/64953960.jpg','https://musify.club/release/ambient-space-sounds-2022-1631776'),
 (9567,28,'The Wanderer&#39;s Dream','2021-04-18 00:00:00',0,'https://38s.musify.club/img/68/23691420/60259228.jpg','https://musify.club/release/god-body-disconnect-the-wanderers-dream-2021-1481408'),
 (9568,28,'The Dormancy','2022-01-20 00:00:00',0,'https://40s.musify.club/img/69/24950842/63004120.jpg','https://musify.club/release/god-body-disconnect-the-dormancy-2021-1567458'),
 (9569,28,'Yoga And Deep House Meditation Music','2020-01-12 00:00:00',0,'https://38s-a.musify.club/img/70/20255664/52047655.jpg','https://musify.club/release/yoga-and-deep-house-meditation-music-2020-1239233'),
 (9570,28,'The Depths of Finality','2020-09-21 00:00:00',0,'https://38s.musify.club/img/68/22133949/57388370.jpg','https://musify.club/release/god-body-disconnect-the-depths-of-finality-2020-1377429'),
 (9571,28,'The Mist Between Mirrors','2022-05-31 00:00:00',0,'https://41s.musify.club/img/68/25834564/64761459.jpg','https://musify.club/release/god-body-disconnect-the-mist-between-mirrors-2019-1625926'),
 (9572,28,'Miles To Midnight','2018-01-11 00:00:00',0,'https://41s-a.musify.club/img/71/15500601/39122643.jpg','https://musify.club/release/miles-to-midnight-2018-951618'),
 (9573,28,'Sleeper&#39;s Fate','2017-06-30 00:00:00',0,'https://41s.musify.club/img/69/14065089/36002975.jpg','https://musify.club/release/god-body-disconnect-sleepers-fate-2017-867872'),
 (9574,28,'Dredge Portals','2016-01-19 00:00:00',0,'https://38s-a.musify.club/img/70/10670882/28315255.jpg','https://musify.club/release/god-body-disconnect-dredge-portals-2016-679749'),
 (9575,28,'Locus Arcadia','2017-09-22 00:00:00',0,'https://41s-a.musify.club/img/71/14684039/37373892.jpg','https://musify.club/release/locus-arcadia-2016-909014'),
 (9576,27,'Colossus','2022-02-16 00:00:00',0,'https://41s.musify.club/img/69/25255682/63389353.jpg','https://musify.club/release/colossus-2022-1581781'),
 (9577,27,'Mortal Shell - OST','2020-08-26 00:00:00',0,'https://38s.musify.club/img/68/21889367/55824064.jpg','https://musify.club/release/atrium-carceri-mortal-shell-ost-2020-1357699'),
 (9578,27,'Codex','2018-09-26 00:00:00',0,'https://41s-a.musify.club/img/70/17224248/45526380.jpg','https://musify.club/release/atrium-carceri-codex-2018-1049018'),
 (9579,27,'Ur Djupan Dal','2018-01-25 00:00:00',0,'https://41s-a.musify.club/img/71/25779429/64558846.jpg','https://musify.club/release/ur-djupan-dal-2018-956310'),
 (9580,27,'Miles To Midnight','2018-01-11 00:00:00',0,'https://41s-a.musify.club/img/71/15500601/39122643.jpg','https://musify.club/release/miles-to-midnight-2018-951618'),
 (9581,27,'Echo','2017-01-23 00:00:00',0,'https://41s.musify.club/img/68/12873395/33324340.jpg','https://musify.club/release/echo-2017-806169'),
 (9582,27,'Black Corner Den','2017-09-18 00:00:00',0,'https://41s-a.musify.club/img/70/14659188/37312527.jpg','https://musify.club/release/black-corner-den-2017-907270'),
 (9583,27,'Archives I-II','2016-12-03 00:00:00',0,'https://41s.musify.club/img/68/12566150/32668855.jpg','https://musify.club/release/atrium-carceri-archives-i-ii-2016-789045'),
 (9584,27,'Onyx','2015-12-25 00:00:00',0,'https://39s.musify.club/img/68/10539080/27280371.jpg','https://musify.club/release/onyx-2015-673556'),
 (9585,27,'Metropolis','2015-06-14 00:00:00',0,'https://39s.musify.club/img/69/9629353/25342009.jpg','https://musify.club/release/atrium-carceri-metropolis-2015-626589'),
 (9586,27,'The Old City: Leviathan - OST','2015-03-12 00:00:00',0,'https://37s.musify.club/img/69/9119245/24266801.jpg','https://musify.club/release/atrium-carceri-the-old-city-leviathan-ost-2015-604482'),
 (9587,27,'Wind Of Buri-Moments Of Life 098 (Dark Ambient Mix)','2016-07-05 00:00:00',0,'https://40s.musify.club/img/69/11692607/30814358.jpg','https://musify.club/release/wind-of-buri-moments-of-life-098-dark-ambient-mix-2014-741583'),
 (9588,27,'The Untold','2013-11-15 00:00:00',0,'https://39s-a.musify.club/img/70/6002966/18190362.jpg','https://musify.club/release/atrium-carceri-the-untold-2013-438519'),
 (9589,27,'Sacrosanct','2012-08-14 00:00:00',0,'https://41s-a.musify.club/img/70/3826917/39610553.jpg','https://musify.club/release/sacrosanct-2012-321843'),
 (9590,27,'Behind The Canvas Of Time','2013-02-05 00:00:00',0,'https://39s.musify.club/img/69/4610020/15330427.jpg','https://musify.club/release/behind-the-canvas-of-time-2012-361325'),
 (9591,27,'Reliquiae','2012-04-03 00:00:00',0,'https://41s.musify.club/img/68/3385585/41351098.jpg','https://musify.club/release/atrium-carceri-reliquiae-2012-294760'),
 (9592,27,'Void','2012-09-22 00:00:00',0,'https://38s.musify.club/img/69/3973574/16906309.jpg','https://musify.club/release/atrium-carceri-void-2011-332969'),
 (9593,27,'Saur Maas / Auditory Dark Matter','2021-05-22 00:00:00',0,'https://39s.musify.club/img/68/23918765/60735413.jpg','https://musify.club/release/saur-maas-auditory-dark-matter-2011-1496289'),
 (9594,27,'Phrenitis','2019-10-17 00:00:00',0,'https://40s-a.musify.club/img/71/19792134/51087854.jpg','https://musify.club/release/atrium-carceri-phrenitis-2009-1214752'),
 (9595,27,'Phrenitis','2011-06-06 00:00:00',0,'https://41s-a.musify.club/img/70/1172175/40217480.jpg','https://musify.club/release/atrium-carceri-phrenitis-2009-218648'),
 (9596,27,'Souyuan','2009-03-03 00:00:00',0,'https://41s-a.musify.club/img/70/816125/40576279.jpg','https://musify.club/release/atrium-carceri-souyuan-2008-28494'),
 (9597,27,'Ptahil','2009-03-03 00:00:00',0,'https://41s-a.musify.club/img/70/816110/40576294.jpg','https://musify.club/release/atrium-carceri-ptahil-2007-28489'),
 (9598,27,'All My Dead Friends','2019-03-27 00:00:00',0,'https://37s.musify.club/img/69/18443199/48253386.jpg','https://musify.club/release/all-my-dead-friends-2006-1124277'),
 (9599,27,'Cold Hands Seduction Vol. 45 (CD2)','2022-02-26 00:00:00',0,'https://40s-a.musify.club/img/70/25300194/63572236.jpg','https://musify.club/release/cold-hands-seduction-vol-45-cd2-2005-1589820'),
 (9600,27,'Kapnobatai','2008-08-04 00:00:00',0,'https://38s.musify.club/img/68/23867732/60617348.jpg','https://musify.club/release/atrium-carceri-kapnobatai-2005-18656'),
 (9601,27,'Seishinbyouin','2009-03-03 00:00:00',0,'https://41s-a.musify.club/img/70/816101/40576303.jpg','https://musify.club/release/atrium-carceri-seishinbyouin-2004-28486'),
 (9602,27,'Flowers Made Of Snow CD2','2011-11-13 00:00:00',0,'https://41s-a.musify.club/img/70/760921/40630398.jpg','https://musify.club/release/flowers-made-of-snow-cd2-2004-256780'),
 (9603,27,'Cellblock','2009-03-03 00:00:00',0,'https://41s-a.musify.club/img/70/816092/40576312.jpg','https://musify.club/release/atrium-carceri-cellblock-2003-28483'),
 (9604,26,'Life Beyond Chapter 2','2021-03-04 00:00:00',0,'https://38s.musify.club/img/68/23344385/59519649.jpg','https://musify.club/release/melodysheep-life-beyond-chapter-2-2020-1455127'),
 (9605,26,'The Music Of Sound','2021-03-04 00:00:00',0,'https://38s.musify.club/img/68/23344459/59519704.jpg','https://musify.club/release/melodysheep-the-music-of-sound-2020-1455132'),
 (9606,26,'The Secret History Of The Moon','2021-03-04 00:00:00',0,'https://38s.musify.club/img/68/23344433/59519679.jpg','https://musify.club/release/melodysheep-the-secret-history-of-the-moon-2020-1455129'),
 (9607,26,'The Arrow of Time: Soundtrack to &quot;Timelapse of the Future&quot;','2019-08-11 00:00:00',0,'https://40s-a.musify.club/img/71/19347901/50132443.jpg','https://musify.club/release/melodysheep-the-arrow-of-time-soundtrack-to-timelapse-of-the-future-2019-1184740'),
 (9608,26,'Life Beyond, Chapter 1','2021-03-04 00:00:00',0,'https://38s.musify.club/img/68/23344497/59519749.jpg','https://musify.club/release/melodysheep-life-beyond-chapter-1-2019-1455135'),
 (9609,26,'Midnight Sun','2021-03-04 00:00:00',0,'https://38s.musify.club/img/68/23344562/59519810.jpg','https://musify.club/release/melodysheep-midnight-sun-2014-1455139'),
 (9610,26,'Remixes For The Soul, Vol. 2','2021-03-04 00:00:00',0,'https://38s.musify.club/img/68/23344597/59519857.jpg','https://musify.club/release/melodysheep-remixes-for-the-soul-vol-2-2013-1455143'),
 (9611,26,'Remixes For The Soul','2021-03-04 00:00:00',0,'https://37s-a.musify.club/img/68/23344632/59520184.jpg','https://musify.club/release/remixes-for-the-soul-2011-1455147'),
 (9612,23,'The Atmosphere Harmony: Ambient Antistress Collection PT.1','2022-05-05 00:00:00',0,'https://40s-a.musify.club/img/70/25725463/64456099.jpg','https://musify.club/release/the-atmosphere-harmony-ambient-antistress-collection-pt-1-2022-1617103'),
 (9613,23,'Find Your Balance: Music For Deep Meditation PT.2','2022-05-05 00:00:00',0,'https://40s-a.musify.club/img/70/25725459/64456092.jpg','https://musify.club/release/find-your-balance-music-for-deep-meditation-pt-2-2022-1617102'),
 (9614,23,'Tomb Of Primordials','2022-05-10 00:00:00',0,'https://37s.musify.club/img/69/25737981/64493197.jpg','https://musify.club/release/tomb-of-primordials-2022-1617903'),
 (9615,23,'Ambient Space Sounds','2022-06-22 00:00:00',0,'https://39s.musify.club/img/68/25958615/64953960.jpg','https://musify.club/release/ambient-space-sounds-2022-1631776'),
 (9616,23,'Adrift On The Edge Of Infinity','2021-12-07 00:00:00',0,'https://37s.musify.club/img/69/24891971/62784611.jpg','https://musify.club/release/dahlias-tear-adrift-on-the-edge-of-infinity-2021-1561035'),
 (9617,23,'Descendants Of The Moon','2020-10-15 00:00:00',0,'https://38s-a.musify.club/img/71/22324397/57592572.jpg','https://musify.club/release/dahlias-tear-descendants-of-the-moon-2020-1388759'),
 (9618,23,'Across the Shifting Abyss','2020-03-30 00:00:00',0,'https://39s-a.musify.club/img/71/20754794/53113362.jpg','https://musify.club/release/dahlias-tear-across-the-shifting-abyss-2019-1268658'),
 (9619,23,'Through The Nightfall Grandeur','2018-10-25 00:00:00',0,'https://37s-a.musify.club/img/71/17417144/46019156.jpg','https://musify.club/release/dahlias-tear-through-the-nightfall-grandeur-2018-1059873'),
 (9620,23,'Behind The Canvas Of Time','2013-02-05 00:00:00',0,'https://39s.musify.club/img/69/4610020/15330427.jpg','https://musify.club/release/behind-the-canvas-of-time-2012-361325'),
 (9621,23,'Dreamsphere','2012-11-14 00:00:00',0,'https://39s.musify.club/img/69/4189626/15378248.jpg','https://musify.club/release/dahlias-tear-dreamsphere-2012-343228'),
 (9622,23,'Krtrima Sprha','2012-09-29 00:00:00',0,'https://38s.musify.club/img/69/4001574/16911541.jpg','https://musify.club/release/krtrima-sprha-2012-334190'),
 (9623,23,'IV: Eridanus Supervoid','2015-02-13 00:00:00',0,'https://39s-a.musify.club/img/71/8937347/23942607.jpg','https://musify.club/release/iv-eridanus-supervoid-2011-596499'),
 (9624,23,'Field Recordings Compilation Vol. 1','2013-12-18 00:00:00',0,'https://39s-a.musify.club/img/71/6208105/27852275.jpg','https://musify.club/release/field-recordings-compilation-vol-1-2010-456462'),
 (9625,23,'Cold Hands Seduction Vol. 80 (CD2)','2022-02-26 00:00:00',0,'https://40s.musify.club/img/69/25311840/63577261.jpg','https://musify.club/release/cold-hands-seduction-vol-80-cd2-2008-1590093'),
 (9626,23,'Under Seven Skies','2009-05-04 00:00:00',0,'https://41s-a.musify.club/img/70/831272/40560889.jpg','https://musify.club/release/dahlias-tear-under-seven-skies-2007-33541'),
 (9627,23,'My Rotten Spirit Of Black','2009-05-04 00:00:00',0,'https://41s-a.musify.club/img/70/831266/40560895.jpg','https://musify.club/release/dahlias-tear-my-rotten-spirit-of-black-2007-33539'),
 (9628,23,'Harmonious Euphonies Supernatural Traumas Mesmerising Our Existences In Radient Corpuscle Galaxies','2009-04-20 00:00:00',0,'https://41s-a.musify.club/img/70/828971/40563185.jpg','https://musify.club/release/dahlias-tear-harmonious-euphonies-supernatural-traumas-mesmerising-our-existences-i-2005-32774'),
 (9629,22,'Davidian','2022-05-31 00:00:00',0,'https://41s.musify.club/img/68/25833152/64761430.jpg','https://musify.club/release/council-of-nine-davidian-2019-1625923'),
 (9630,22,'Exit Earth','2018-06-28 00:00:00',0,'https://38s-a.musify.club/img/70/16582744/44087890.jpg','https://musify.club/release/council-of-nine-exit-earth-2018-1012203'),
 (9631,22,'Tomb Of Seers','2017-02-18 00:00:00',0,'https://41s.musify.club/img/69/13092618/33779164.jpg','https://musify.club/release/tomb-of-seers-2017-817695'),
 (9632,22,'Trinity','2017-07-13 00:00:00',0,'https://41s.musify.club/img/69/14155844/36216829.jpg','https://musify.club/release/council-of-nine-trinity-2017-874632'),
 (9633,22,'Terra Relicta Presents: Vol. I Dark Ambient','2017-09-28 00:00:00',0,'https://41s-a.musify.club/img/71/14729222/37491546.jpg','https://musify.club/release/terra-relicta-presents-vol-i-dark-ambient-2016-911789'),
 (9634,22,'Locus Arcadia','2017-09-22 00:00:00',0,'https://41s-a.musify.club/img/71/14684039/37373892.jpg','https://musify.club/release/locus-arcadia-2016-909014'),
 (9635,22,'Diagnosis','2017-07-24 00:00:00',0,'https://41s.musify.club/img/69/14244765/36400680.jpg','https://musify.club/release/council-of-nine-diagnosis-2015-880206'),
 (9636,22,'Dakhma','2015-05-05 00:00:00',0,'https://39s.musify.club/img/69/9435735/24969000.jpg','https://musify.club/release/council-of-nine-dakhma-2015-618016'),
 (9637,22,'Tomb Of Empires','2015-09-15 00:00:00',0,'https://39s.musify.club/img/68/10045927/26222997.jpg','https://musify.club/release/tomb-of-empires-2014-647398'),
 (9638,21,'Beta Pictoris','2021-05-22 00:00:00',0,'https://39s.musify.club/img/68/23916529/60727671.jpg','https://musify.club/release/sphare-sechs-beta-pictoris-2021-1495890'),
 (9639,21,'Transient Lunar Phenomenon','2020-08-25 00:00:00',0,'https://37s-a.musify.club/img/68/21877138/55629092.jpg','https://musify.club/release/sphare-sechs-transient-lunar-phenomenon-2019-1355455'),
 (9640,21,'Particle Void','2018-04-19 00:00:00',0,'https://41s-a.musify.club/img/70/16101712/42998116.jpg','https://musify.club/release/sphare-sechs-particle-void-2018-985564'),
 (9641,21,'Enceladus','2015-12-07 00:00:00',0,'https://40s-a.musify.club/img/70/10459197/27087903.jpg','https://musify.club/release/sphare-sechs-enceladus-2015-668060'),
 (9642,21,'Malignant Antibodies','2012-09-20 00:00:00',0,'https://39s.musify.club/img/69/3962140/15403680.jpg','https://musify.club/release/malignant-antibodies-2012-329514'),
 (9643,21,'Tiefschlaf [Limited Edition]','2012-11-30 00:00:00',0,'https://39s.musify.club/img/69/4255092/15505228.jpg','https://musify.club/release/sphare-sechs-tiefschlaf-limited-edition-2012-346463'),
 (9644,21,'Krtrima Sprha','2012-09-29 00:00:00',0,'https://38s.musify.club/img/69/4001574/16911541.jpg','https://musify.club/release/krtrima-sprha-2012-334190'),
 (9645,20,'Globalsect Radio','2020-08-01 00:00:00',0,'https://40s-a.musify.club/img/71/21734058/55388901.jpg','https://musify.club/release/globalsect-radio-2020-1329242'),
 (9646,20,'Spirit of Goa Trance Vol. 2','2017-07-12 00:00:00',0,'https://41s.musify.club/img/68/14153067/36203741.jpg','https://musify.club/release/spirit-of-goa-trance-vol-2-2017-874251'),
 (9647,20,'Terraformer (6 Tracks Preview)','2017-04-20 00:00:00',0,'https://41s.musify.club/img/68/13580296/35004898.jpg','https://musify.club/release/terraformer-6-tracks-preview-2017-843000'),
 (9648,20,'Psychedelic Euphoria Infinite Realites Future Trance','2017-08-08 00:00:00',0,'https://41s.musify.club/img/69/14336992/36599469.jpg','https://musify.club/release/psychedelic-euphoria-infinite-realites-future-trance-2017-886655'),
 (9649,20,'Selection Of The Finest Psytrance Chapter 3','2017-09-19 00:00:00',0,'https://41s-a.musify.club/img/71/14663703/37330171.jpg','https://musify.club/release/selection-of-the-finest-psytrance-chapter-3-2017-907715'),
 (9650,20,'Terraformer','2017-06-03 00:00:00',0,'https://41s.musify.club/img/68/13906743/35684174.jpg','https://musify.club/release/terraformer-2017-860171'),
 (9651,20,'Selection Of The Finest Psytrance Chapter 1','2017-09-19 00:00:00',0,'https://41s-a.musify.club/img/71/14663693/37330165.jpg','https://musify.club/release/selection-of-the-finest-psytrance-chapter-1-2017-907714'),
 (9652,20,'Selection Of The Finest Psytrance Chapter 2','2017-09-22 00:00:00',0,'https://41s-a.musify.club/img/71/14663699/37360760.jpg','https://musify.club/release/selection-of-the-finest-psytrance-chapter-2-2017-908253'),
 (9653,20,'The Mystery of Crystal Worlds','2016-01-07 00:00:00',0,'https://38s-a.musify.club/img/70/10612771/27601708.jpg','https://musify.club/release/the-mystery-of-crystal-worlds-2015-677354'),
 (9654,20,'101 Goa Trance Masters Hits DJ Mix 2015','2020-12-06 00:00:00',0,'https://38s.musify.club/img/68/22701019/58176027.jpg','https://musify.club/release/101-goa-trance-masters-hits-dj-mix-2015-2015-1410104'),
 (9655,20,'The Mystery of Crystal Worlds: Prologue','2015-10-09 00:00:00',0,'https://39s.musify.club/img/69/10156611/26476718.jpg','https://musify.club/release/the-mystery-of-crystal-worlds-prologue-2015-653023'),
 (9656,20,'Space Of Power: The Legend About Great Existence Of The Universe [CD 1]','2012-11-10 00:00:00',0,'https://39s.musify.club/img/69/4169294/15413523.jpg','https://musify.club/release/space-of-power-the-legend-about-great-existence-of-the-universe-cd-1-2012-342058'),
 (9657,20,'Space Of Power: The Legend About Great Existence Of The Universe [CD 2]','2012-11-10 00:00:00',0,'https://39s.musify.club/img/69/4169297/15371247.jpg','https://musify.club/release/space-of-power-the-legend-about-great-existence-of-the-universe-cd-2-2012-342059'),
 (9658,20,'Dance of Distant Worlds','2017-04-21 00:00:00',0,'https://41s.musify.club/img/68/13594137/35039447.jpg','https://musify.club/release/dance-of-distant-worlds-2012-844239'),
 (9659,19,'Stay At Home Sessions 1','2021-07-23 00:00:00',0,'https://40s-a.musify.club/img/70/24316754/61568214.jpg','https://musify.club/release/mental-discipline-stay-at-home-sessions-1-2021-1524591'),
 (9660,19,'VA The Heroes Of Electronics 2','2022-01-31 00:00:00',0,'https://41s-a.musify.club/img/71/25059184/63148463.jpg','https://musify.club/release/va-the-heroes-of-electronics-2-2021-1572878'),
 (9661,19,'Cold (Dark) W... Night Vol. 5','2020-12-01 00:00:00',0,'https://38s.musify.club/img/68/22649108/58071563.jpg','https://musify.club/release/cold-dark-w-night-vol-5-2020-1406894'),
 (9662,19,'World Of Electronic Music Vol.5 Part 2','2019-12-17 00:00:00',0,'https://38s-a.musify.club/img/70/20149239/51819488.jpg','https://musify.club/release/world-of-electronic-music-vol-5-part-2-2019-1234099'),
 (9663,19,'World Of Electronic Music Vol.5 Part 1','2019-12-17 00:00:00',0,'https://38s-a.musify.club/img/70/20149236/51819408.jpg','https://musify.club/release/world-of-electronic-music-vol-5-part-1-2019-1234098'),
 (9664,19,'Synth Waves Vol.4 Lyrics','2019-03-27 00:00:00',0,'https://37s.musify.club/img/69/18443616/48253809.jpg','https://musify.club/release/synth-waves-vol-4-lyrics-2019-1123843'),
 (9665,19,'World Of Electronic Music Vol. 4 Part 1','2019-12-16 00:00:00',0,'https://39s.musify.club/img/69/20138663/51809683.jpg','https://musify.club/release/world-of-electronic-music-vol-4-part-1-2019-1233715'),
 (9666,19,'World Of Electronic Music Vol.1 [Part 2]','2019-12-11 00:00:00',0,'https://40s-a.musify.club/img/71/20109670/51757906.jpg','https://musify.club/release/world-of-electronic-music-vol-1-part-2-2019-1232482'),
 (9667,19,'Ocean Of Love New Generation Italo Disco','2018-04-13 00:00:00',0,'https://41s-a.musify.club/img/71/16052655/42899152.jpg','https://musify.club/release/ocean-of-love-new-generation-italo-disco-2018-982663'),
 (9668,19,'Past Forward','2018-12-01 00:00:00',0,'https://37s-a.musify.club/img/71/17650987/46428390.jpg','https://musify.club/release/mental-discipline-past-forward-2018-1073392'),
 (9669,19,'Fallen Stars Redux','2018-06-09 00:00:00',0,'https://41s-a.musify.club/img/71/16438198/43727062.jpg','https://musify.club/release/mental-discipline-fallen-stars-redux-2018-1005264'),
 (9670,19,'Lifekiller','2018-04-21 00:00:00',0,'https://41s-a.musify.club/img/71/16116201/43035379.jpg','https://musify.club/release/lifekiller-2018-986804'),
 (9671,19,'Andromeda: Glamtronica Remix II','2019-06-11 00:00:00',0,'https://40s-a.musify.club/img/71/18954877/49286322.jpg','https://musify.club/release/andromeda-glamtronica-remix-ii-2018-1156399'),
 (9672,19,'Synth Waves Vol.1','2017-04-16 00:00:00',0,'https://41s.musify.club/img/68/13543407/34946264.jpg','https://musify.club/release/synth-waves-vol-1-2017-841583'),
 (9673,19,'Electronic Synthspace In Genre Italo Disco','2017-08-30 00:00:00',0,'https://41s-a.musify.club/img/70/14515072/37003437.jpg','https://musify.club/release/electronic-synthspace-in-genre-italo-disco-2017-898739'),
 (9674,19,'Synth Waves Vol.2','2017-04-20 00:00:00',0,'https://41s.musify.club/img/68/13575711/35003092.jpg','https://musify.club/release/synth-waves-vol-2-2017-842921'),
 (9675,19,'Synth Waves Vol.3 Prototype','2017-05-10 00:00:00',0,'https://41s.musify.club/img/68/13738531/35333665.jpg','https://musify.club/release/synth-waves-vol-3-prototype-2017-851500'),
 (9676,19,'New Edition Italo Disco CD 2','2016-09-21 00:00:00',0,'https://37s.musify.club/img/69/12121475/31724771.jpg','https://musify.club/release/new-edition-italo-disco-cd-2-2016-764872'),
 (9677,19,'New Sounds &amp; More Power Vol. 10','2016-04-21 00:00:00',0,'https://38s-a.musify.club/img/70/11294748/29964318.jpg','https://musify.club/release/new-sounds-and-more-power-vol-10-2016-718264'),
 (9678,19,'Precious Paradise','2016-05-15 00:00:00',0,'https://40s-a.musify.club/img/71/19021458/49399150.jpg','https://musify.club/release/mental-discipline-precious-paradise-2016-726736'),
 (9679,19,'End Of Days','2016-03-21 00:00:00',0,'https://40s-a.musify.club/img/71/19021454/49399133.jpg','https://musify.club/release/mental-discipline-end-of-days-2016-707526'),
 (9680,19,'A Tribute To DEPECHE MODE','2016-04-06 00:00:00',0,'https://38s-a.musify.club/img/70/11206323/29779855.jpg','https://musify.club/release/a-tribute-to-depeche-mode-2016-712827'),
 (9681,19,'Re​/​Vision: The Skyqode Tribute To De​/​Vision','2015-07-08 00:00:00',0,'https://39s.musify.club/img/69/9657563/25557360.jpg','https://musify.club/release/re-vision-the-skyqode-tribute-to-de-vision-2015-631525'),
 (9682,19,'Italo &amp; Space Vol. 14','2015-09-01 00:00:00',0,'https://37s.musify.club/img/69/9996958/26123174.jpg','https://musify.club/release/italo-and-space-vol-14-2015-645188'),
 (9683,19,'Resistanz Festival Soundtrack 2015','2015-03-28 00:00:00',0,'https://37s.musify.club/img/69/9210792/24452901.jpg','https://musify.club/release/resistanz-festival-soundtrack-2015-2015-608699'),
 (9684,19,'New Sounds &amp; More Power Vol. 2','2016-03-02 00:00:00',0,'https://40s.musify.club/img/69/10987594/29323466.jpg','https://musify.club/release/new-sounds-and-more-power-vol-2-2015-700165'),
 (9685,19,'Butterfly','2014-03-25 00:00:00',0,'https://38s-a.musify.club/img/70/6916538/19849813.jpg','https://musify.club/release/mental-discipline-butterfly-2014-484873'),
 (9686,19,'Neonautics Vol.1','2014-12-25 00:00:00',0,'https://39s-a.musify.club/img/71/8726028/23294335.jpg','https://musify.club/release/neonautics-vol-1-2014-585413'),
 (9687,19,'Enjoy The Sounds 20.5 All The Best','2016-09-18 00:00:00',0,'https://37s.musify.club/img/69/12104903/31691098.jpg','https://musify.club/release/enjoy-the-sounds-20-5-all-the-best-2013-764072'),
 (9688,19,'Fall 2 Pieces','2013-08-04 00:00:00',0,'https://37s-a.musify.club/img/68/5423655/16322745.jpg','https://musify.club/release/mental-discipline-fall-2-pieces-2013-394291'),
 (9689,19,'Depechematika : An Anniversary Tribute To Depeche Mode','2013-08-30 00:00:00',0,'https://40s.musify.club/img/69/5185022/16818027.jpg','https://musify.club/release/depechematika-an-anniversary-tribute-to-depeche-mode-2012-398028'),
 (9690,19,'Electropop 7','2017-10-18 00:00:00',0,'https://41s-a.musify.club/img/71/9672051/37812394.jpg','https://musify.club/release/electropop-7-2012-919944'),
 (9691,19,'Constellation','2020-04-03 00:00:00',0,'https://39s-a.musify.club/img/70/20790162/53174301.jpg','https://musify.club/release/mental-discipline-constellation-2012-1270762'),
 (9692,19,'Constellation','2013-01-22 00:00:00',0,'https://37s.musify.club/img/69/9612449/25265907.jpg','https://musify.club/release/mental-discipline-constellation-2012-358094'),
 (9693,19,'Synthematika Four','2014-06-09 00:00:00',0,'https://40s.musify.club/img/69/7451579/20913162.jpg','https://musify.club/release/synthematika-four-2012-510978'),
 (9694,19,'Fall To Pieces (Single)','2018-05-17 00:00:00',0,'https://37s-a.musify.club/img/71/16252651/43367624.jpg','https://musify.club/release/mental-discipline-fall-to-pieces-single-2010-995037'),
 (9695,19,'SYNTHEMATIKA TWO','2011-06-17 00:00:00',0,'https://41s-a.musify.club/img/70/1182141/40207470.jpg','https://musify.club/release/synthematika-two-2010-221969'),
 (9696,19,'Kill Emotions','2017-02-09 00:00:00',0,'https://41s.musify.club/img/69/12993536/33585085.jpg','https://musify.club/release/mental-discipline-kill-emotions-2009-813113'),
 (9697,18,'Sainted by the Storm','2022-03-28 00:00:00',0,'https://38s.musify.club/img/68/25486140/63951880.jpg','https://musify.club/release/powerwolf-sainted-by-the-storm-2022-1603780'),
 (9698,18,'Cold Hands Seduction Vol. 230','2022-02-16 00:00:00',0,'https://41s.musify.club/img/68/25254408/63387820.jpg','https://musify.club/release/cold-hands-seduction-vol-230-2021-1581715'),
 (9699,18,'Bete Du Gevaudan','2021-12-06 00:00:00',0,'https://37s-a.musify.club/img/71/24875925/62774320.jpg','https://musify.club/release/powerwolf-bete-du-gevaudan-2021-1560528'),
 (9700,18,'Call Of The Wild','2021-07-15 00:00:00',0,'https://37s-a.musify.club/img/68/24268346/61474667.jpg','https://musify.club/release/powerwolf-call-of-the-wild-2021-1522092'),
 (9701,18,'Fist by Fist (Sacralize or Strike) ','2021-07-14 00:00:00',0,'https://38s-a.musify.club/img/71/24258315/61451119.jpg','https://musify.club/release/fist-by-fist-sacralize-or-strike-2021-1521449'),
 (9702,18,'Malleo Metalum (Metal Hammer Promo CD)','2021-06-17 00:00:00',0,'https://38s-a.musify.club/img/71/24109854/61142065.jpg','https://musify.club/release/powerwolf-malleo-metalum-metal-hammer-promo-cd-2021-1509126'),
 (9703,18,'Beast of G&#233;vaudan','2021-05-24 00:00:00',0,'https://39s.musify.club/img/68/23933175/60763479.jpg','https://musify.club/release/powerwolf-beast-of-gevaudan-2021-1496809'),
 (9704,18,'Demons Are A Girl&#39;s Best Friend ','2021-06-10 00:00:00',0,'https://39s-a.musify.club/img/71/24076356/61065964.jpg','https://musify.club/release/powerwolf-demons-are-a-girls-best-friend-2021-1506448'),
 (9705,18,'Dancing With The Dead','2021-06-25 00:00:00',0,'https://38s.musify.club/img/68/24159229/61237557.jpg','https://musify.club/release/powerwolf-dancing-with-the-dead-2021-1512764'),
 (9706,18,'Любимые Баллады Разных Времён - Часть 11','2021-07-20 00:00:00',0,'https://38s.musify.club/img/68/24285968/61516432.jpg','https://musify.club/release/lubimie-balladi-raznih-vremen-chast-11-2021-1523041'),
 (9707,18,'Heavy Metal Collections Vol. 17','2020-02-09 00:00:00',0,'https://39s.musify.club/img/69/20340828/52387609.jpg','https://musify.club/release/heavy-metal-collections-vol-17-2020-1248364'),
 (9708,18,'Heavy Metal Collections Vol. 15','2020-02-09 00:00:00',0,'https://39s-a.musify.club/img/70/20340822/52387193.jpg','https://musify.club/release/heavy-metal-collections-vol-15-2020-1248361'),
 (9709,18,'Heavy Metal Collections Vol. 7','2020-01-28 00:00:00',0,'https://38s.musify.club/img/69/20331113/52192034.jpg','https://musify.club/release/heavy-metal-collections-vol-7-2020-1241934'),
 (9710,18,'Midnight Madonna','2020-12-04 00:00:00',0,'https://38s.musify.club/img/68/22689559/58146294.jpg','https://musify.club/release/powerwolf-midnight-madonna-2020-1409206'),
 (9711,18,'Stossgebet','2020-12-04 00:00:00',0,'https://38s.musify.club/img/68/22689643/58146317.jpg','https://musify.club/release/stossgebet-2020-1409208'),
 (9712,18,' The Symphony of Sin (Orchestral Version)','2020-06-09 00:00:00',0,'https://38s-a.musify.club/img/70/21291807/54270957.jpg','https://musify.club/release/powerwolf-the-symphony-of-sin-orchestral-version-2020-1299738'),
 (9713,18,'Heavy Metal Collections Vol. 10','2020-01-30 00:00:00',0,'https://40s-a.musify.club/img/71/20340835/52209467.jpg','https://musify.club/release/heavy-metal-collections-vol-10-2020-1242334'),
 (9714,18,' Werewolves Of Armenia (Rerecorded Version)','2020-04-01 00:00:00',0,'https://38s.musify.club/img/68/22689575/58145482.jpg','https://musify.club/release/powerwolf-werewolves-of-armenia-rerecorded-version-2020-1269597'),
 (9715,18,'Where The Wild Wolves Have Gone','2020-12-04 00:00:00',0,'https://38s.musify.club/img/68/22689719/58146346.jpg','https://musify.club/release/where-the-wild-wolves-have-gone-2020-1409211'),
 (9716,18,'Summerbreeze 2018 &amp; Bloodstock 2019','2021-03-05 00:00:00',0,'https://38s-a.musify.club/img/71/23355463/59550003.jpg','https://musify.club/release/powerwolf-summerbreeze-2018-and-bloodstock-2019-2020-1455559'),
 (9717,18,'Best of the Blessed ','2020-07-02 00:00:00',0,'https://38s.musify.club/img/69/21510480/54649875.jpg','https://musify.club/release/powerwolf-best-of-the-blessed-2020-1310248'),
 (9718,18,'Heavy Metal Collections Vol. 16','2020-02-09 00:00:00',0,'https://39s-a.musify.club/img/70/20340825/52387311.jpg','https://musify.club/release/heavy-metal-collections-vol-16-2020-1248362'),
 (9719,18,'Heavy Metal Collections','2020-02-11 00:00:00',0,'https://39s.musify.club/img/69/20391991/52436051.jpg','https://musify.club/release/heavy-metal-collections-2020-1250326'),
 (9720,18,'The Best of Heavy Metal','2020-08-08 00:00:00',0,'https://38s.musify.club/img/69/21774566/55499935.jpg','https://musify.club/release/the-best-of-heavy-metal-2019-1336575'),
 (9721,18,'The Best of Heavy Metal','2020-08-12 00:00:00',0,'https://40s-a.musify.club/img/71/21796587/55523070.jpg','https://musify.club/release/the-best-of-heavy-metal-2019-1340303'),
 (9722,18,'A Tribute To Judas Priest (Metal Hammer)','2019-03-15 00:00:00',0,'https://38s-a.musify.club/img/70/18357668/48144386.jpg','https://musify.club/release/a-tribute-to-judas-priest-metal-hammer-2019-1118262'),
 (9723,18,'I Love Metal Music','2019-10-06 00:00:00',0,'https://40s-a.musify.club/img/71/19708173/50901963.jpg','https://musify.club/release/i-love-metal-music-2019-1209065'),
 (9724,18,'The Best World Rock Hits Part 2','2019-11-02 00:00:00',0,'https://39s-a.musify.club/img/70/19902656/51315137.jpg','https://musify.club/release/the-best-world-rock-hits-part-2-2019-1220993'),
 (9725,18,'Metallum Nostrum','2019-01-10 00:00:00',0,'https://38s.musify.club/img/69/17897233/47128732.jpg','https://musify.club/release/powerwolf-metallum-nostrum-2019-1090575'),
 (9726,18,'Out In The Fields ','2019-10-09 00:00:00',0,'https://40s-a.musify.club/img/71/19726339/50939890.jpg','https://musify.club/release/powerwolf-out-in-the-fields-2019-1210091'),
 (9727,18,'Kiss of the Cobra King (New Version)','2019-11-12 00:00:00',0,'https://39s-a.musify.club/img/70/19966579/51439256.jpg','https://musify.club/release/powerwolf-kiss-of-the-cobra-king-new-version-2019-1224194'),
 (9728,18,'Power Metal Ballads 2018 CD1','2019-01-22 00:00:00',0,'https://38s.musify.club/img/69/17966666/47215523.jpg','https://musify.club/release/power-metal-ballads-2018-cd1-2019-1095502'),
 (9729,18,'The Best of Heavy Metal','2020-08-12 00:00:00',0,'https://40s-a.musify.club/img/71/21797052/55523616.jpg','https://musify.club/release/the-best-of-heavy-metal-2019-1340357'),
 (9730,18,'Live At Masters Of Rock','2021-03-05 00:00:00',0,'https://37s-a.musify.club/img/68/23347742/59532256.jpg','https://musify.club/release/powerwolf-live-at-masters-of-rock-2019-1455560'),
 (9731,18,'The Sacrilege Symphony II','2020-12-04 00:00:00',0,'https://38s.musify.club/img/68/22689538/58146267.jpg','https://musify.club/release/powerwolf-the-sacrilege-symphony-ii-2019-1409204'),
 (9732,18,'Best Of Metal','2019-03-04 00:00:00',0,'https://41s-a.musify.club/img/70/18280901/47987330.jpg','https://musify.club/release/best-of-metal-2019-1113615'),
 (9733,18,'VA - Heavy Metal Is A Religion (Covers Compilation) (CD 8)','2018-11-16 00:00:00',0,'https://40s-a.musify.club/img/71/19473915/50309736.jpg','https://musify.club/release/va-heavy-metal-is-a-religion-covers-compilation-cd-8-2018-1069414'),
 (9734,18,'Hell&#39;s Torments Vol.2','2018-11-05 00:00:00',0,'https://37s-a.musify.club/img/68/17494109/46119273.jpg','https://musify.club/release/hells-torments-vol-2-2018-1065131'),
 (9735,18,'Hard - Rock Attack Vol.32','2018-09-10 00:00:00',0,'https://40s-a.musify.club/img/70/17349227/45745260.jpg','https://musify.club/release/hard-rock-attack-vol-32-2018-1042716'),
 (9736,18,'1001% Of ROCK','2018-11-26 00:00:00',0,'https://37s-a.musify.club/img/71/17605559/46320604.jpg','https://musify.club/release/1001-of-rock-2018-1070570'),
 (9737,18,'German Metal: Gold I Gave For Iron Vol.1','2018-06-12 00:00:00',0,'https://37s.musify.club/img/69/16455716/43767043.jpg','https://musify.club/release/german-metal-gold-i-gave-for-iron-vol-1-2018-1006602'),
 (9738,18,'Metal All Stars','2018-12-15 00:00:00',0,'https://39s-a.musify.club/img/70/17729062/46861237.jpg','https://musify.club/release/metal-all-stars-2018-1079603'),
 (9739,18,'VA - Heavy Metal Is A Religion (Covers Compilation) (CD 7)','2018-11-26 00:00:00',0,'https://41s.musify.club/img/69/25525212/64020440.jpg','https://musify.club/release/va-heavy-metal-is-a-religion-covers-compilation-cd-7-2018-1070236'),
 (9740,18,'VA - Heavy Metal Is A Religion (Covers Compilation) (CD 6)','2018-11-17 00:00:00',0,'https://41s.musify.club/img/69/25525218/64020452.jpg','https://musify.club/release/va-heavy-metal-is-a-religion-covers-compilation-cd-6-2018-1069836'),
 (9741,18,'The Sacrament Of Sin','2018-07-20 00:00:00',0,'https://40s-a.musify.club/img/70/16724048/44364412.jpg','https://musify.club/release/the-sacrament-of-sin-2018-1020458'),
 (9742,18,'Masters Of Rock','2019-12-26 00:00:00',0,'https://39s-a.musify.club/img/70/20245490/51995115.jpg','https://musify.club/release/masters-of-rock-2018-1235697'),
 (9743,18,'Demons Are A Girl‘S Best Friend','2018-05-26 00:00:00',0,'https://41s.musify.club/img/69/16338189/43507471.jpg','https://musify.club/release/powerwolf-demons-are-a-girl-s-best-friend-2018-999076'),
 (9744,18,'Power Metal Ballads 2018','2019-03-15 00:00:00',0,'https://37s.musify.club/img/69/18345950/48071786.jpg','https://musify.club/release/power-metal-ballads-2018-2018-1118592'),
 (9745,18,'Incense &amp; Iron','2018-07-16 00:00:00',0,'https://38s.musify.club/img/69/16868776/44673376.jpg','https://musify.club/release/powerwolf-incense-and-iron-2018-1019548'),
 (9746,18,'Fire &amp; Forgive','2018-06-21 00:00:00',0,'https://40s.musify.club/img/69/16521780/43894668.jpg','https://musify.club/release/powerwolf-fire-and-forgive-2018-1009679'),
 (9747,18,'VA - Heavy Metal Is A Religion (Covers Compilation) (CD 9)','2018-11-16 00:00:00',0,'https://41s.musify.club/img/69/25525215/64020446.jpg','https://musify.club/release/va-heavy-metal-is-a-religion-covers-compilation-cd-9-2018-1069413'),
 (9748,18,'Hell&#39;s Torments Vol.1','2018-11-05 00:00:00',0,'https://37s-a.musify.club/img/68/17494688/46119604.jpg','https://musify.club/release/hells-torments-vol-1-2018-1065327'),
 (9749,18,'Satan Is Real','2017-04-30 00:00:00',0,'https://41s.musify.club/img/69/13659130/35173083.jpg','https://musify.club/release/satan-is-real-2017-847390'),
 (9750,18,'Evil Tribute To Judas Priest, Vol.2','2017-07-13 00:00:00',0,'https://41s.musify.club/img/69/14160351/36219879.jpg','https://musify.club/release/evil-tribute-to-judas-priest-vol-2-2017-874817'),
 (9751,18,'Metal Hammer Goes 90s','2020-07-29 00:00:00',0,'https://38s.musify.club/img/69/21709390/55040534.jpg','https://musify.club/release/metal-hammer-goes-90s-2017-1325879'),
 (9752,18,'A Trbute To Iron Maiden ','2017-05-07 00:00:00',0,'https://41s.musify.club/img/68/13724382/35298856.jpg','https://musify.club/release/a-trbute-to-iron-maiden-2017-850327'),
 (9753,18,'Metal-Hard Rock Covers 727','2017-04-26 00:00:00',0,'https://41s.musify.club/img/68/13631652/35102527.jpg','https://musify.club/release/metal-hard-rock-covers-727-2017-845500'),
 (9754,18,'Preaching At The Breeze','2017-07-25 00:00:00',0,'https://41s.musify.club/img/68/14254698/36413302.jpg','https://musify.club/release/powerwolf-preaching-at-the-breeze-2017-880410'),
 (9755,18,'Evil Tribute To Black Sabbath, Vol.3','2017-06-16 00:00:00',0,'https://41s.musify.club/img/68/13991049/35852707.jpg','https://musify.club/release/evil-tribute-to-black-sabbath-vol-3-2017-864504'),
 (9756,18,'I Love Music! Heavy Metal Edition Vol.34','2017-06-11 00:00:00',0,'https://41s.musify.club/img/68/13963796/35798190.jpg','https://musify.club/release/i-love-music-heavy-metal-edition-vol-34-2017-863277'),
 (9757,18,'Blessed &amp; Orchestrated','2020-12-04 00:00:00',0,'https://38s.musify.club/img/68/22689517/58146243.jpg','https://musify.club/release/powerwolf-blessed-and-orchestrated-2017-1409202'),
 (9758,18,'Symphonic Metal - Dark &amp; Beautiful Vol 11 (CD1)','2018-01-02 00:00:00',0,'https://41s-a.musify.club/img/71/15448412/39011717.jpg','https://musify.club/release/symphonic-metal-dark-and-beautiful-vol-11-cd1-2017-948812'),
 (9759,18,'Symphonic &amp; Opera Metal vol.3','2017-03-06 00:00:00',0,'https://41s.musify.club/img/68/13230406/34080344.jpg','https://musify.club/release/symphonic-and-opera-metal-vol-3-2017-825335'),
 (9760,18,'Symphonic &amp; Opera Metal Vol.1','2017-03-07 00:00:00',0,'https://41s.musify.club/img/69/13231293/34081291.jpg','https://musify.club/release/symphonic-and-opera-metal-vol-1-2017-825482'),
 (9761,18,'Evil Tribute To Ozzy Osbourne','2017-06-10 00:00:00',0,'https://41s.musify.club/img/68/13957612/35780459.jpg','https://musify.club/release/evil-tribute-to-ozzy-osbourne-2017-862734'),
 (9762,18,'The World Of Heavy Metal Vol.3 (CD2)','2017-12-29 00:00:00',0,'https://41s-a.musify.club/img/71/16044319/42870726.jpg','https://musify.club/release/the-world-of-heavy-metal-vol-3-cd2-2017-947072'),
 (9763,18,'Various Artists - Iron Maiden - The 80&#39;s Covered','2016-09-10 00:00:00',0,'https://39s.musify.club/img/69/12056127/31591159.jpg','https://musify.club/release/various-artists-iron-maiden-the-80s-covered-2016-760693'),
 (9764,18,'Metal &amp; Rock Collection','2016-02-14 00:00:00',0,'https://39s-a.musify.club/img/70/10883907/29095778.jpg','https://musify.club/release/metal-and-rock-collection-2016-693539'),
 (9765,18,'Fury-Metal [ Metal Collection ]','2018-02-18 00:00:00',0,'https://38s-a.musify.club/img/70/15717365/42122728.jpg','https://musify.club/release/fury-metal-metal-collection-2016-963968'),
 (9766,18,'The Metal Mass-Live','2016-07-31 00:00:00',0,'https://39s.musify.club/img/69/11818223/31079279.jpg','https://musify.club/release/powerwolf-the-metal-mass-live-2016-747377'),
 (9767,18,'Dark &amp; Beautiful X','2017-02-02 00:00:00',0,'https://41s.musify.club/img/68/12939178/33469782.jpg','https://musify.club/release/dark-and-beautiful-x-2016-809931'),
 (9768,18,'Стальные Нервы CD4','2017-02-09 00:00:00',0,'https://41s.musify.club/img/69/12992264/33579637.jpg','https://musify.club/release/stalnie-nervi-cd4-2016-812762'),
 (9769,18,'Symphonic &amp; Opera Metal Vol.2','2017-03-07 00:00:00',0,'https://41s.musify.club/img/69/13230985/34080975.jpg','https://musify.club/release/symphonic-and-opera-metal-vol-2-2016-825862'),
 (9770,18,'Brothers Of Metal','2016-09-04 00:00:00',0,'https://40s-a.musify.club/img/70/12022882/31522007.jpg','https://musify.club/release/brothers-of-metal-2016-758986'),
 (9771,18,'Dark &amp; Beautiful IX','2017-02-02 00:00:00',0,'https://41s.musify.club/img/69/12938536/33475275.jpg','https://musify.club/release/dark-and-beautiful-ix-2015-810338'),
 (9772,18,'Army Of The Night','2020-12-04 00:00:00',0,'https://38s.musify.club/img/68/22689450/58146160.jpg','https://musify.club/release/powerwolf-army-of-the-night-2015-1409196'),
 (9773,18,'  РОК В АВТО (Vol. 10)','2015-11-18 00:00:00',0,'https://40s-a.musify.club/img/70/10363780/26896473.jpg','https://musify.club/release/rok-v-avto-vol-10-2015-663935'),
 (9774,18,'Armata Strigoi','2015-06-12 00:00:00',0,'https://39s-a.musify.club/img/71/9613442/25319872.jpg','https://musify.club/release/powerwolf-armata-strigoi-2015-625947'),
 (9775,18,'HMR Vol.4','2016-12-27 00:00:00',0,'https://41s.musify.club/img/68/12689820/32954826.jpg','https://musify.club/release/hmr-vol-4-2015-796313'),
 (9776,18,'Blessed &amp; Possessed','2015-07-12 00:00:00',0,'https://39s.musify.club/img/68/9795493/25649625.jpg','https://musify.club/release/powerwolf-blessed-and-possessed-2015-633168'),
 (9777,18,'Wolfsn&#228;chte 2015 Tour EP','2016-06-10 00:00:00',0,'https://38s-a.musify.club/img/70/11558526/30558896.jpg','https://musify.club/release/wolfsnachte-2015-tour-ep-2015-733552'),
 (9778,18,'Melodic Metal Anthems Vol.11','2015-04-19 00:00:00',0,'https://39s.musify.club/img/68/9315810/24677505.jpg','https://musify.club/release/melodic-metal-anthems-vol-11-2014-614429'),
 (9779,18,'Melodic Metal Anthems Vol.3','2015-04-19 00:00:00',0,'https://39s-a.musify.club/img/71/9315573/24676822.jpg','https://musify.club/release/melodic-metal-anthems-vol-3-2014-614368'),
 (9780,18,'Metal-Hard Rock Covers 666','2015-01-10 00:00:00',0,'https://39s-a.musify.club/img/71/8782648/23429263.jpg','https://musify.club/release/metal-hard-rock-covers-666-2014-588690'),
 (9781,18,'Symphonic Metal - Dark &amp; Beautiful VIII (2CD)','2015-08-30 00:00:00',0,'https://37s.musify.club/img/69/9993866/26107700.jpg','https://musify.club/release/symphonic-metal-dark-and-beautiful-viii-2cd-2014-644809'),
 (9782,18,'Symphonic Metal - Dark &amp; Beautiful VII (2CD)','2015-08-30 00:00:00',0,'https://37s.musify.club/img/69/9993586/26107291.jpg','https://musify.club/release/symphonic-metal-dark-and-beautiful-vii-2cd-2014-644804'),
 (9783,18,'Boneyard Metal Volume XIX','2015-09-15 00:00:00',0,'https://39s.musify.club/img/68/10045565/26222919.jpg','https://musify.club/release/boneyard-metal-volume-xix-2014-647395'),
 (9784,18,'ANTHEMS','2014-10-22 00:00:00',0,'https://37s.musify.club/img/69/8246877/22466853.jpg','https://musify.club/release/anthems-2014-564967'),
 (9785,18,'Preachers Of The Night','2013-07-21 00:00:00',0,'https://37s-a.musify.club/img/68/5316921/15756170.jpg','https://musify.club/release/powerwolf-preachers-of-the-night-2013-392428'),
 (9786,18,'The Rockhard Sacrament','2013-07-17 00:00:00',0,'https://37s-a.musify.club/img/68/5299793/15695484.jpg','https://musify.club/release/powerwolf-the-rockhard-sacrament-2013-392235'),
 (9787,18,'Amen &amp; Attack','2016-06-10 00:00:00',0,'https://38s-a.musify.club/img/70/11558403/30558758.jpg','https://musify.club/release/powerwolf-amen-and-attack-2013-733568'),
 (9788,18,'Symphonic Metal - Dark &amp; Beautiful VI (CD1)','2015-08-30 00:00:00',0,'https://37s.musify.club/img/69/9994084/26107925.jpg','https://musify.club/release/symphonic-metal-dark-and-beautiful-vi-cd1-2013-644802'),
 (9789,18,'Symphonic Metal - Dark &amp; Beautiful VI (CD2)','2015-08-30 00:00:00',0,'https://41s.musify.club/img/68/12937610/33453931.jpg','https://musify.club/release/symphonic-metal-dark-and-beautiful-vi-cd2-2013-644791'),
 (9790,18,'Metal-Hard Rock Covers 637','2016-11-30 00:00:00',0,'https://41s.musify.club/img/68/12530657/32620318.jpg','https://musify.club/release/metal-hard-rock-covers-637-2013-787653'),
 (9791,18,'Wolfsnaechte 2012 Tour EP','2016-06-10 00:00:00',0,'https://38s-a.musify.club/img/70/11558465/30558826.jpg','https://musify.club/release/wolfsnaechte-2012-tour-ep-2012-733566'),
 (9792,18,'Alive In The Night','2012-04-03 00:00:00',0,'https://41s.musify.club/img/68/3387027/41350292.jpg','https://musify.club/release/powerwolf-alive-in-the-night-2012-294887'),
 (9793,18,'EP IN BLOODRED','2013-09-15 00:00:00',0,'https://40s-a.musify.club/img/70/5328483/17233503.jpg','https://musify.club/release/powerwolf-ep-in-bloodred-2011-413527'),
 (9794,18,'Power Metal Compilation','2011-12-02 00:00:00',0,'https://37s.musify.club/img/69/1780956/28626174.jpg','https://musify.club/release/power-metal-compilation-2011-261371'),
 (9795,18,'Greatest Hits','2011-12-06 00:00:00',0,'https://39s.musify.club/img/69/1811384/15466818.jpg','https://musify.club/release/powerwolf-greatest-hits-2011-263026'),
 (9796,18,'Power Metal Vol. 2','2011-09-15 00:00:00',0,'https://41s-a.musify.club/img/70/1250367/40137852.jpg','https://musify.club/release/power-metal-vol-2-2011-244716'),
 (9797,18,'We Drink Your Blood','2020-12-05 00:00:00',0,'https://40s.musify.club/img/68/22689348/58157003.jpg','https://musify.club/release/powerwolf-we-drink-your-blood-2011-1409410'),
 (9798,18,'Blood Of The Saints','2011-08-01 00:00:00',0,'https://38s-a.musify.club/img/70/16259374/43328533.jpg','https://musify.club/release/powerwolf-blood-of-the-saints-2011-238013'),
 (9799,18,'Dark World Heavy Compilation Vol.1','2012-05-02 00:00:00',0,'https://41s.musify.club/img/68/3490551/41300125.jpg','https://musify.club/release/dark-world-heavy-compilation-vol-1-2009-301834'),
 (9800,18,'Bible Of The Beast','2009-04-17 00:00:00',0,'https://41s-a.musify.club/img/70/14821831/37649669.jpg','https://musify.club/release/powerwolf-bible-of-the-beast-2009-32593'),
 (9801,18,'ReUnation - A Tribute To Running Wild [CD1]','2011-11-16 00:00:00',0,'https://41s-a.musify.club/img/70/1128903/40261676.jpg','https://musify.club/release/reunation-a-tribute-to-running-wild-cd1-2009-257799'),
 (9802,18,'Let The Hammer Fall, Vol.76','2009-06-25 00:00:00',0,'https://41s-a.musify.club/img/70/1122975/40267585.jpg','https://musify.club/release/let-the-hammer-fall-vol-76-2009-202287'),
 (9803,18,'Northern Warriors - Power Metal Compilation Vol.2','2011-09-11 00:00:00',0,'https://41s-a.musify.club/img/70/1247133/40141092.jpg','https://musify.club/release/northern-warriors-power-metal-compilation-vol-2-2008-243637'),
 (9804,18,'Lupus Dei','2009-08-10 00:00:00',0,'https://41s-a.musify.club/img/70/852254/40539949.jpg','https://musify.club/release/powerwolf-lupus-dei-2007-40537'),
 (9805,18,'Return In Bloodred','2009-08-10 00:00:00',0,'https://41s-a.musify.club/img/70/852251/40539952.jpg','https://musify.club/release/powerwolf-return-in-bloodred-2005-40536'),
 (9806,17,'Symphonies II','2022-03-12 00:00:00',0,'https://38s.musify.club/img/68/25380544/63732891.jpg','https://musify.club/release/neurotech-symphonies-ii-2022-1597429'),
 (9807,17,'Deceive [Solace Bonus Track]','2022-03-12 00:00:00',0,'https://38s.musify.club/img/68/25380557/63732901.jpg','https://musify.club/release/neurotech-deceive-solace-bonus-track-2021-1597430'),
 (9808,17,'Solace','2021-03-18 00:00:00',0,'https://38s-a.musify.club/img/71/23433076/59718211.jpg','https://musify.club/release/neurotech-solace-2021-1463624'),
 (9809,17,'Unreleased Demos (2011 - 2016)','2021-04-08 00:00:00',0,'https://39s.musify.club/img/68/23608155/60051803.jpg','https://musify.club/release/neurotech-unreleased-demos-2011-2016-2020-1473461'),
 (9810,17,'The Elysian Symphony','2021-04-08 00:00:00',0,'https://39s.musify.club/img/68/23608326/60053095.jpg','https://musify.club/release/neurotech-the-elysian-symphony-2017-1473520'),
 (9811,17,'Remixes','2021-04-08 00:00:00',0,'https://39s.musify.club/img/68/23608275/60053060.jpg','https://musify.club/release/neurotech-remixes-2017-1473516'),
 (9812,17,'Our Burial Ground (Synths Sound Design)','2021-04-08 00:00:00',0,'https://39s.musify.club/img/68/23608267/60053053.jpg','https://musify.club/release/neurotech-our-burial-ground-synths-sound-design-2017-1473515'),
 (9813,17,'The Catalyst (Vocals &amp; Synths)','2021-04-08 00:00:00',0,'https://39s.musify.club/img/68/23608305/60053078.jpg','https://musify.club/release/neurotech-the-catalyst-vocals-and-synths-2017-1473518'),
 (9814,17,'The Doors - Strange Days (Neurotech Cover)','2021-04-12 00:00:00',0,'https://38s.musify.club/img/68/23608322/60154750.jpg','https://musify.club/release/neurotech-the-doors-strange-days-neurotech-cover-2017-1477811'),
 (9815,17,'Licensed Songs','2021-04-08 00:00:00',0,'https://39s.musify.club/img/68/23608256/60053042.jpg','https://musify.club/release/neurotech-licensed-songs-2017-1473514'),
 (9816,17,'The Catalyst','2017-06-23 00:00:00',0,'https://41s.musify.club/img/68/14026884/35918443.jpg','https://musify.club/release/neurotech-the-catalyst-2017-866019'),
 (9817,17,'Symphonies','2016-12-26 00:00:00',0,'https://41s.musify.club/img/68/12697616/32944449.jpg','https://musify.club/release/neurotech-symphonies-2016-795889'),
 (9818,17,'Стальные Нервы CD3','2017-02-09 00:00:00',0,'https://41s.musify.club/img/69/12992246/33579623.jpg','https://musify.club/release/stalnie-nervi-cd3-2016-812763'),
 (9819,17,'Trust To Display (Beta Version)','2021-04-08 00:00:00',0,'https://39s.musify.club/img/68/23608339/60053111.jpg','https://musify.club/release/neurotech-trust-to-display-beta-version-2016-1473522'),
 (9820,17,'In Remission','2016-06-05 00:00:00',0,'https://38s-a.musify.club/img/70/11527143/30452597.jpg','https://musify.club/release/neurotech-in-remission-2016-731464'),
 (9821,17,'Ectogenesis','2021-04-08 00:00:00',0,'https://39s.musify.club/img/68/23608245/60053028.jpg','https://musify.club/release/neurotech-ectogenesis-2015-1473512'),
 (9822,17,'Cut The Cord (Unfinished Track)','2021-04-08 00:00:00',0,'https://39s.musify.club/img/68/23608216/60053021.jpg','https://musify.club/release/neurotech-cut-the-cord-unfinished-track-2015-1473511'),
 (9823,17,'The Ophidian Symphony','2016-01-01 00:00:00',0,'https://37s.musify.club/img/69/10579133/27419724.jpg','https://musify.club/release/neurotech-the-ophidian-symphony-2015-675910'),
 (9824,17,'I Desensitize','2016-03-26 00:00:00',0,'https://39s.musify.club/img/68/11143350/29651068.jpg','https://musify.club/release/neurotech-i-desensitize-2015-709478'),
 (9825,17,'Stigma','2015-06-14 00:00:00',0,'https://37s.musify.club/img/69/9627090/25340729.jpg','https://musify.club/release/neurotech-stigma-2015-626555'),
 (9826,17,'Evasive','2015-11-14 00:00:00',0,'https://40s-a.musify.club/img/70/10356721/26851855.jpg','https://musify.club/release/neurotech-evasive-2015-662957'),
 (9827,17,'The Halcyon Symphony','2014-12-28 00:00:00',0,'https://37s.musify.club/img/69/8737196/23314395.jpg','https://musify.club/release/neurotech-the-halcyon-symphony-2014-585954'),
 (9828,17,'Infra Versus Ultra','2014-10-31 00:00:00',0,'https://39s.musify.club/img/68/8338449/22576353.jpg','https://musify.club/release/neurotech-infra-versus-ultra-2014-567847'),
 (9829,17,'Various Demos','2021-04-08 00:00:00',0,'https://39s.musify.club/img/68/23608131/60052971.jpg','https://musify.club/release/neurotech-various-demos-2014-1473506'),
 (9830,17,'Hope Is Better Than A Memory','2021-04-08 00:00:00',0,'https://39s.musify.club/img/68/23608250/60053035.jpg','https://musify.club/release/neurotech-hope-is-better-than-a-memory-2013-1473513'),
 (9831,17,'The Decipher Volumes','2013-06-28 00:00:00',0,'https://37s-a.musify.club/img/68/5293469/15529771.jpg','https://musify.club/release/neurotech-the-decipher-volumes-2013-391531'),
 (9832,17,'The Cyber Waltz','2021-04-08 00:00:00',0,'https://39s.musify.club/img/68/23608315/60053085.jpg','https://musify.club/release/neurotech-the-cyber-waltz-2013-1473519'),
 (9833,17,'The Elysian Symphony','2013-12-26 00:00:00',0,'https://39s-a.musify.club/img/70/6347295/18738526.jpg','https://musify.club/release/neurotech-the-elysian-symphony-2013-458451'),
 (9834,17,'Unconditional','2021-04-08 00:00:00',0,'https://39s.musify.club/img/68/23608124/60052964.jpg','https://musify.club/release/neurotech-unconditional-2012-1473505'),
 (9835,17,'Decipher Vol. 2','2012-11-26 00:00:00',0,'https://39s.musify.club/img/69/4231940/15505967.jpg','https://musify.club/release/neurotech-decipher-vol-2-2012-345529'),
 (9836,17,'Decipher Vol. 1','2012-04-28 00:00:00',0,'https://41s.musify.club/img/68/3472847/41308301.jpg','https://musify.club/release/neurotech-decipher-vol-1-2012-300657'),
 (9837,17,'Futuristic Metal Compilation: Cyber Metal','2011-12-06 00:00:00',0,'https://37s.musify.club/img/69/1809492/28560963.jpg','https://musify.club/release/futuristic-metal-compilation-cyber-metal-2011-262897'),
 (9838,17,'Coda','2021-04-08 00:00:00',0,'https://39s.musify.club/img/68/23608195/60053014.jpg','https://musify.club/release/neurotech-coda-2011-1473510'),
 (9839,17,'Blue Screen Planet','2012-01-07 00:00:00',0,'https://39s.musify.club/img/69/2176059/16972244.jpg','https://musify.club/release/neurotech-blue-screen-planet-2011-272703'),
 (9840,17,'Blue Screen Planet Extras','2021-04-08 00:00:00',0,'https://39s.musify.club/img/68/23608186/60053004.jpg','https://musify.club/release/neurotech-blue-screen-planet-extras-2011-1473509'),
 (9841,17,'Antagonist','2011-04-09 00:00:00',0,'https://41s-a.musify.club/img/70/1139028/40251503.jpg','https://musify.club/release/neurotech-antagonist-2011-207629'),
 (9842,17,'Batch 1','2021-04-08 00:00:00',0,'https://39s.musify.club/img/68/23608173/60052992.jpg','https://musify.club/release/neurotech-batch-1-2009-1473508'),
 (9843,17,'The Angst Zeit (Piano Strings Version)','2021-04-08 00:00:00',0,'https://39s.musify.club/img/68/23608289/60053071.jpg','https://musify.club/release/neurotech-the-angst-zeit-piano-strings-version-2009-1473517'),
 (9844,17,'We Are The Last','2021-04-08 00:00:00',0,'https://39s.musify.club/img/68/23608361/60053136.jpg','https://musify.club/release/neurotech-we-are-the-last-2008-1473524'),
 (9845,17,'Transhuman','2011-11-09 00:00:00',0,'https://41s-a.musify.club/img/70/746683/40644094.jpg','https://musify.club/release/neurotech-transhuman-2008-255850'),
 (9846,17,'Stuff','2021-04-08 00:00:00',0,'https://39s.musify.club/img/68/23608167/60052983.jpg','https://musify.club/release/neurotech-stuff-2007-1473507'),
 (9847,16,'METAL, ROCK - Любимые Хиты Разных Лет - Часть 15','2022-02-16 00:00:00',0,'https://41s.musify.club/img/69/25251324/63375908.jpg','https://musify.club/release/metal-rock-lubimie-hiti-raznih-let-chast-15-2022-1581213'),
 (9848,16,'Yours','2022-03-19 00:00:00',0,'https://41s.musify.club/img/69/25421133/63827541.jpg','https://musify.club/release/dynazty-yours-2022-1600713'),
 (9849,16,'METAL, ROCK - Любимые Хиты Разных Годов - Часть 19','2022-03-18 00:00:00',0,'https://38s.musify.club/img/68/25398487/63800757.jpg','https://musify.club/release/metal-rock-lubimie-hiti-raznih-godov-chast-19-2022-1599450'),
 (9850,16,'METAL, ROCK - Любимые Хиты Разных Годов - Часть 26','2022-05-17 00:00:00',0,'https://40s.musify.club/img/69/25789263/64599739.jpg','https://musify.club/release/metal-rock-lubimie-hiti-raznih-godov-chast-26-2022-1621949'),
 (9851,16,'METAL, ROCK - Любимые Хиты Разных Лет - Часть 16','2022-02-21 00:00:00',0,'https://37s.musify.club/img/69/25282656/63492577.jpg','https://musify.club/release/metal-rock-lubimie-hiti-raznih-let-chast-16-2022-1586225'),
 (9852,16,'METAL, ROCK - Любимые Хиты Разных Лет - Часть 13','2022-01-20 00:00:00',0,'https://40s.musify.club/img/69/25109368/62995333.jpg','https://musify.club/release/metal-rock-lubimie-hiti-raznih-let-chast-13-2022-1567063'),
 (9853,16,'Power Of Will','2022-01-22 00:00:00',0,'https://37s.musify.club/img/69/24977010/63031113.jpg','https://musify.club/release/dynazty-power-of-will-2021-1568523'),
 (9854,16,'25 Years - Metal Addiction','2022-04-10 00:00:00',0,'https://40s.musify.club/img/69/25552397/64121391.jpg','https://musify.club/release/25-years-metal-addiction-2021-1608010'),
 (9855,16,'Advent','2022-01-20 00:00:00',0,'https://38s.musify.club/img/68/24954275/63005767.jpg','https://musify.club/release/dynazty-advent-2021-1567539'),
 (9856,16,'Heavy Metal Collections Vol. 17 CD 5','2020-12-21 00:00:00',0,'https://40s.musify.club/img/68/22816125/58416420.jpg','https://musify.club/release/heavy-metal-collections-vol-17-cd-5-2020-1417817'),
 (9857,16,'Heavy Metal Collections Vol. 10','2020-01-30 00:00:00',0,'https://40s-a.musify.club/img/71/20340835/52209467.jpg','https://musify.club/release/heavy-metal-collections-vol-10-2020-1242334'),
 (9858,16,'Rock Ballads About The Sublime Part 1','2020-06-10 00:00:00',0,'https://38s-a.musify.club/img/70/21326948/54309152.jpg','https://musify.club/release/rock-ballads-about-the-sublime-part-1-2020-1301627'),
 (9859,16,'Heavy Metal Collections Vol. 13','2020-02-09 00:00:00',0,'https://39s-a.musify.club/img/70/20340815/52386965.jpg','https://musify.club/release/heavy-metal-collections-vol-13-2020-1248359'),
 (9860,16,'Heavy Metal Collections Vol. 17 CD 2','2020-12-21 00:00:00',0,'https://40s.musify.club/img/68/22816115/58416402.jpg','https://musify.club/release/heavy-metal-collections-vol-17-cd-2-2020-1417814'),
 (9861,16,'Heavy Metal Collections Vol. 16','2020-02-09 00:00:00',0,'https://39s-a.musify.club/img/70/20340825/52387311.jpg','https://musify.club/release/heavy-metal-collections-vol-16-2020-1248362'),
 (9862,16,'Presence Of Mind','2020-02-04 00:00:00',0,'https://38s-a.musify.club/img/70/20354222/52280136.jpg','https://musify.club/release/dynazty-presence-of-mind-2020-1242785'),
 (9863,16,'The Dark Delight','2020-04-01 00:00:00',0,'https://38s-a.musify.club/img/70/20782634/53139961.jpg','https://musify.club/release/dynazty-the-dark-delight-2020-1269587'),
 (9864,16,'The Best of Heavy Metal','2020-08-12 00:00:00',0,'https://40s-a.musify.club/img/71/21796587/55523070.jpg','https://musify.club/release/the-best-of-heavy-metal-2019-1340303'),
 (9865,16,'Phoenix Rising (CD1)','2019-09-01 00:00:00',0,'https://40s-a.musify.club/img/71/19438313/50370062.jpg','https://musify.club/release/phoenix-rising-cd1-2019-1190507'),
 (9866,16,'Heavy Metal Collections Vol. 13 CD 1','2019-09-25 00:00:00',0,'https://40s-a.musify.club/img/71/19636020/50750757.jpg','https://musify.club/release/heavy-metal-collections-vol-13-cd-1-2019-1203685'),
 (9867,16,'Breathe With Me','2018-08-08 00:00:00',0,'https://37s.musify.club/img/69/16846849/44659543.jpg','https://musify.club/release/dynazty-breathe-with-me-2018-1027851'),
 (9868,16,'Firesign','2018-09-28 00:00:00',0,'https://41s-a.musify.club/img/70/17234152/45539093.jpg','https://musify.club/release/dynazty-firesign-2018-1049768'),
 (9869,16,'Beautiful Rock Ballads Vol.10','2017-08-10 00:00:00',0,'https://37s-a.musify.club/img/71/14353229/36638287.jpg','https://musify.club/release/beautiful-rock-ballads-vol-10-2017-887750'),
 (9870,16,'Beautiful Rock Ballads Vol.9','2017-08-09 00:00:00',0,'https://37s-a.musify.club/img/71/14353162/36630441.jpg','https://musify.club/release/beautiful-rock-ballads-vol-9-2017-887578'),
 (9871,16,'Heavy Metal Edition Vol.31','2017-02-25 00:00:00',0,'https://41s.musify.club/img/69/13138917/33887034.jpg','https://musify.club/release/heavy-metal-edition-vol-31-2017-820726'),
 (9872,16,'Hard Rock Mania Vol 15','2017-02-18 00:00:00',0,'https://41s.musify.club/img/69/13085058/33765907.jpg','https://musify.club/release/hard-rock-mania-vol-15-2017-817534'),
 (9873,16,'Titanic Mass','2016-04-14 00:00:00',0,'https://38s-a.musify.club/img/70/11259694/29876071.jpg','https://musify.club/release/dynazty-titanic-mass-2016-715512'),
 (9874,16,'Roar Of The Underdog','2016-02-12 00:00:00',0,'https://38s-a.musify.club/img/70/10865218/29057456.jpg','https://musify.club/release/dynazty-roar-of-the-underdog-2016-692339'),
 (9875,16,'POWER METAL Drive 4','2016-07-06 00:00:00',0,'https://40s.musify.club/img/69/11705049/30827780.jpg','https://musify.club/release/power-metal-drive-4-2016-742055'),
 (9876,16,'Hard-Rock Attack Vol.20','2016-07-21 00:00:00',0,'https://40s.musify.club/img/69/11766088/30962165.jpg','https://musify.club/release/hard-rock-attack-vol-20-2016-745303'),
 (9877,16,'POWER METAL Drive 1','2016-05-11 00:00:00',0,'https://39s-a.musify.club/img/70/11410976/30203257.jpg','https://musify.club/release/power-metal-drive-1-2016-724887'),
 (9878,16,'The Heavier Side Of The Eurovision','2015-12-28 00:00:00',0,'https://39s.musify.club/img/68/10571497/27307539.jpg','https://musify.club/release/the-heavier-side-of-the-eurovision-2015-674527'),
 (9879,16,'Hard&#39;n&#39;heavy Vol.04','2015-06-30 00:00:00',0,'https://39s.musify.club/img/69/9707869/25498888.jpg','https://musify.club/release/hardnheavy-vol-04-2015-630002'),
 (9880,16,'Hard Rock Mania Vol. 12','2015-01-13 00:00:00',0,'https://37s.musify.club/img/69/8810602/23464703.jpg','https://musify.club/release/hard-rock-mania-vol-12-2014-589989'),
 (9881,16,'Melodic Metal Anthems Vol.11','2015-04-19 00:00:00',0,'https://39s.musify.club/img/68/9315810/24677505.jpg','https://musify.club/release/melodic-metal-anthems-vol-11-2014-614429'),
 (9882,16,'New Year Rock Show 2','2014-01-11 00:00:00',0,'https://39s-a.musify.club/img/70/6437681/18919911.jpg','https://musify.club/release/new-year-rock-show-2-2014-462700'),
 (9883,16,'Renatus','2014-03-26 00:00:00',0,'https://38s-a.musify.club/img/70/6924492/19867300.jpg','https://musify.club/release/dynazty-renatus-2014-485243'),
 (9884,16,'Melodic Metal Anthems Vol.2','2015-04-19 00:00:00',0,'https://39s-a.musify.club/img/71/9315550/24676800.jpg','https://musify.club/release/melodic-metal-anthems-vol-2-2014-614367'),
 (9885,16,'Rock Ballads','2013-08-28 00:00:00',0,'https://37s-a.musify.club/img/68/5452838/16378231.jpg','https://musify.club/release/rock-ballads-2013-397563'),
 (9886,16,'Hair Metal 80&#39;S [Vol. 5] [Disc 1]','2014-03-26 00:00:00',0,'https://38s-a.musify.club/img/70/6921506/19859886.jpg','https://musify.club/release/hair-metal-80s-vol-5-disc-1-2013-485162'),
 (9887,16,'Hair Metal 80&#39;s [Vol. 5] [Disc 2]','2014-03-26 00:00:00',0,'https://38s-a.musify.club/img/70/6921606/19860015.jpg','https://musify.club/release/hair-metal-80s-vol-5-disc-2-2013-485200'),
 (9888,16,'FOR YOU','2013-11-30 00:00:00',0,'https://39s-a.musify.club/img/71/6091450/28023132.jpg','https://musify.club/release/for-you-2013-442059'),
 (9889,16,'MADNESS COLLECTION Vol I','2013-11-30 00:00:00',0,'https://39s-a.musify.club/img/71/6091166/28023955.jpg','https://musify.club/release/madness-collection-vol-i-2013-442058'),
 (9890,16,'The Bonus Track Album Vol.03','2013-10-24 00:00:00',0,'https://39s-a.musify.club/img/70/5765738/18001258.jpg','https://musify.club/release/the-bonus-track-album-vol-03-2013-432989'),
 (9891,16,'Madness Collectoin','2013-11-25 00:00:00',0,'https://39s-a.musify.club/img/71/6039323/28081754.jpg','https://musify.club/release/madness-collectoin-2013-440891'),
 (9892,16,'Hair Metal 80&#39;S [Vol. 2] [Disc 1]','2013-09-11 00:00:00',0,'https://40s.musify.club/img/69/5375112/17056297.jpg','https://musify.club/release/hair-metal-80s-vol-2-disc-1-2013-399131'),
 (9893,16,'Melodifestivalen 2012','2018-04-14 00:00:00',0,'https://41s-a.musify.club/img/70/16055173/42917678.jpg','https://musify.club/release/melodifestivalen-2012-2012-983617'),
 (9894,16,'Sultans Of Sin','2012-03-29 00:00:00',0,'https://41s.musify.club/img/68/3376615/41357125.jpg','https://musify.club/release/dynazty-sultans-of-sin-2012-294249'),
 (9895,16,'Let&#39;s Get Rocked Vol.14','2018-02-13 00:00:00',0,'https://40s-a.musify.club/img/70/15674671/42078195.jpg','https://musify.club/release/lets-get-rocked-vol-14-2012-962121'),
 (9896,16,'Let&#39;s Get Rocked Vol. 10','2012-01-20 00:00:00',0,'https://41s-a.musify.club/img/70/2944364/39984128.jpg','https://musify.club/release/lets-get-rocked-vol-10-2011-276816'),
 (9898,16,'Bring The Thunder','2010-04-05 00:00:00',0,'https://41s-a.musify.club/img/70/932345/40459059.jpg','https://musify.club/release/dynazty-bring-the-thunder-2009-67239'),
 (9900,16,'Knock You Down','2011-05-08 00:00:00',0,'https://41s-a.musify.club/img/70/1152048/40237844.jpg','https://musify.club/release/dynazty-knock-you-down-2011-211968'),
 (9901,1,'Underneath - (Single)','2022-07-20',0,'https://f4.bcbits.com/img/a3479145746_2.jpg','https://alphaxone.bandcamp.com/album/underneath-single'),
 (9902,1,'Unperceived - (Single)','2022-07-20',0,'https://f4.bcbits.com/img/a0371203554_2.jpg','https://alphaxone.bandcamp.com/album/unperceived-single'),
 (9903,1,'Dark Complex - [Remastered/24bit]','2022-07-20',0,'https://f4.bcbits.com/img/a2671027820_2.jpg','https://alphaxone.bandcamp.com/album/dark-complex-remastered-24bit'),
 (9904,1,'Endless Horizon - EP','2022-07-20',0,'https://f4.bcbits.com/img/a0472842567_2.jpg','https://alphaxone.bandcamp.com/album/endless-horizon-ep'),
 (9905,1,'Perception - EP','2022-07-20',0,'https://f4.bcbits.com/img/a2109725603_2.jpg','https://alphaxone.bandcamp.com/album/perception-ep'),
 (9906,1,'Synthetic Vision','2022-07-20',0,'https://f4.bcbits.com/img/a3604054128_2.jpg','https://alphaxone.bandcamp.com/album/synthetic-vision'),
 (9907,1,'Invisible - EP','2022-07-20',0,'/img/0.gif','https://alphaxone.bandcamp.com/album/invisible-ep'),
 (9908,1,'Phase.o.n.e - (Single)','2022-07-20',0,'/img/0.gif','https://alphaxone.bandcamp.com/album/phase-o-n-e-single'),
 (9909,1,'Grounds - (Single)','2022-07-20',0,'/img/0.gif','https://alphaxone.bandcamp.com/album/grounds-single'),
 (9910,1,'Portal','2022-07-20',0,'/img/0.gif','https://alphaxone.bandcamp.com/album/portal'),
 (9911,1,'Endurance - (Single)','2022-07-20',0,'/img/0.gif','https://alphaxone.bandcamp.com/album/endurance-single'),
 (9912,2,'Club Vibes Part 01','2022-07-20',0,'https://f4.bcbits.com/img/a1074708920_2.jpg','https://borisbrejcha.bandcamp.com/album/club-vibes-part-01'),
 (9913,2,'Feuerfalter Part 01 Deluxe Edition','2022-07-20',0,'https://f4.bcbits.com/img/a1399504946_2.jpg','https://borisbrejcha.bandcamp.com/album/feuerfalter-part-01-deluxe-edition'),
 (9914,2,'Feuerfalter Part 02 Deluxe Edition','2022-07-20',0,'https://f4.bcbits.com/img/a1365765629_2.jpg','https://borisbrejcha.bandcamp.com/album/feuerfalter-part-02-deluxe-edition'),
 (9915,2,'Die Maschinen sind Gestrandet','2022-07-20',0,'https://f4.bcbits.com/img/a0068361957_2.jpg','https://borisbrejcha.bandcamp.com/album/die-maschinen-sind-gestrandet'),
 (9916,2,'Purple Noise Remixes Part 1','2022-07-20',0,'https://f4.bcbits.com/img/a3636095589_2.jpg','https://borisbrejcha.bandcamp.com/album/purple-noise-remixes-part-1'),
 (9917,2,'Feuerfalter Part 02','2022-07-20',0,'https://f4.bcbits.com/img/a2449874063_2.jpg','https://borisbrejcha.bandcamp.com/album/feuerfalter-part-02'),
 (9918,2,'Feuerfalter Part 01','2022-07-20',0,'/img/0.gif','https://borisbrejcha.bandcamp.com/album/feuerfalter-part-01'),
 (9919,2,'My Name is ...Remixes','2022-07-20',0,'/img/0.gif','https://borisbrejcha.bandcamp.com/album/my-name-is-remixes'),
 (9920,2,'My Name Is','2022-07-20',0,'/img/0.gif','https://borisbrejcha.bandcamp.com/album/my-name-is'),
 (9921,2,'Mein wahres Ich','2022-07-20',0,'/img/0.gif','https://borisbrejcha.bandcamp.com/album/mein-wahres-ich'),
 (9922,2,'Die Maschinen kontrollieren uns','2022-07-20',0,'/img/0.gif','https://borisbrejcha.bandcamp.com/album/die-maschinen-kontrollieren-uns'),
 (9923,3,'.','2022-07-20',0,'https://f4.bcbits.com/img/a0501180076_2.jpg','https://cryochamber.bandcamp.com/album/-'),
 (9924,3,'CD Bundles
                
                
                CD Bundles','2022-07-20',0,'https://f4.bcbits.com/img/a1502446084_2.jpg','https://cryochamber.bandcamp.com/album/cd-bundles'),
 (9925,3,'Phantoms Received
                
                
                Apocryphos','2022-07-20',0,'https://f4.bcbits.com/img/a0774377873_2.jpg','https://cryochamber.bandcamp.com/album/phantoms-received'),
 (9926,3,'Dissolving into Solitary Landscapes
                
                
                Dronny Darko &amp; G M Slater','2022-07-20',0,'https://f4.bcbits.com/img/a2868491666_2.jpg','https://cryochamber.bandcamp.com/album/dissolving-into-solitary-landscapes'),
 (9927,3,'Styx
                
                
                Phonothek','2022-07-20',0,'https://f4.bcbits.com/img/a0554433501_2.jpg','https://cryochamber.bandcamp.com/album/styx'),
 (9928,3,'Lost in Fog
                
                
                Phonothek','2022-07-20',0,'https://f4.bcbits.com/img/a3555956068_2.jpg','https://cryochamber.bandcamp.com/album/lost-in-fog'),
 (9929,3,'Red Moon
                
                
                Phonothek','2022-07-20',0,'https://f4.bcbits.com/img/a0355763347_2.jpg','https://cryochamber.bandcamp.com/album/red-moon'),
 (9930,3,'Vestigium
                
                
                Crypthios','2022-07-20',0,'https://f4.bcbits.com/img/a0442391706_2.jpg','https://cryochamber.bandcamp.com/album/vestigium'),
 (9931,3,'Ruins
                
                
                Void Stasis','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/ruins'),
 (9932,3,'Tomb of Primordials','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/tomb-of-primordials'),
 (9933,3,'Sundown
                
                
                Beyond the Ghost','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/sundown'),
 (9934,3,'Memento
                
                
                Dead Melodies','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/memento'),
 (9935,3,'Memory Alpha
                
                
                ProtoU','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/memory-alpha'),
 (9936,3,'Colossus
                
                
                Atrium Carceri &amp; Kammarheit','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/colossus'),
 (9937,3,'Mothership
                
                
                Tineidae','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/mothership'),
 (9938,3,'Timeless
                
                
                Mindwarden','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/timeless'),
 (9939,3,'Dagon
                
                
                Cryo Chamber Collaboration','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/dagon'),
 (9940,3,'Quasi
                
                
                Dronny Darko &amp; Phaedrus','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/quasi'),
 (9941,3,'The Dormancy
                
                
                God Body Disconnect','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/the-dormancy'),
 (9942,3,'ISIH
                
                
                Metatron Omega','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/isih'),
 (9943,3,'Adrift on the Edge of Infinity
                
                
                Dahlia&#39;s Tear','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/adrift-on-the-edge-of-infinity'),
 (9944,3,'Niflheim
                
                
                Ager Sonus','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/niflheim'),
 (9945,3,'The Desolation Age
                
                
                Beyond the Ghost','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/the-desolation-age'),
 (9946,3,'Fifth Nature
                
                
                Skrika','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/fifth-nature'),
 (9947,3,'Submersion
                
                
                Gdanian','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/submersion'),
 (9948,3,'Creation of a Star
                
                
                Planet Supreme','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/creation-of-a-star'),
 (9949,3,'A Greater Bliss
                
                
                Wordclock','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/a-greater-bliss'),
 (9950,3,'The Night and Other Sunken Dreams
                
                
                Underwater Sleep Orchestra','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/the-night-and-other-sunken-dreams'),
 (9951,3,'Fabled Machines of Old
                
                
                Dead Melodies','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/fabled-machines-of-old'),
 (9952,3,'The Umbra Report
                
                
                Cities Last Broadcast','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/the-umbra-report'),
 (9953,3,'Beta Pictoris
                
                
                Sphäre Sechs','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/beta-pictoris'),
 (9954,3,'Back to Beyond
                
                
                Alphaxone &amp; ProtoU','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/back-to-beyond'),
 (9955,3,'Reflections at the Sea
                
                
                SiJ &amp; Textere Oris','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/reflections-at-the-sea'),
 (9956,3,'The Wanderer&#39;s Dream
                
                
                God Body Disconnect','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/the-wanderers-dream'),
 (9957,3,'Shore Rituals
                
                
                Ruptured World','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/shore-rituals'),
 (9958,3,'Division Cycle
                
                
                Hilyard','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/division-cycle'),
 (9959,3,'Radioactive Immersion
                
                
                Dronny Darko &amp; Ajna','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/radioactive-immersion'),
 (9960,3,'Tomb of Wights','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/tomb-of-wights'),
 (9961,3,'The Last Resort
                
                
                Beyond the Ghost','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/the-last-resort'),
 (9962,3,'2149
                
                
                Sabled Sun','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/2149'),
 (9963,3,'Dark Ambient of 2020','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/dark-ambient-of-2020'),
 (9964,3,'Yig
                
                
                Cryo Chamber Collaboration','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/yig'),
 (9965,3,'Ghost Machine
                
                
                Alphaxone','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/ghost-machine'),
 (9966,3,'Infernal Beyond
                
                
                Flowers for Bodysnatchers','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/infernal-beyond'),
 (9967,3,'Advent
                
                
                Randal Collier-Ford','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/advent'),
 (9968,3,'Crier&#39;s Bane
                
                
                Dead Melodies &amp; Beyond the Ghost','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/criers-bane'),
 (9969,3,'Mortal Shell Soundtrack
                
                
                Atrium Carceri','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/mortal-shell-soundtrack'),
 (9970,3,'Freedom and Loneliness
                
                
                Ugasanie','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/freedom-and-loneliness'),
 (9971,3,'Descendants of the Moon
                
                
                Dahlia&#39;s Tear','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/descendants-of-the-moon'),
 (9972,3,'Nusquam
                
                
                Aegri Somnia','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/nusquam'),
 (9973,3,'The Depths of Finality
                
                
                God Body Disconnect','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/the-depths-of-finality'),
 (9974,3,'Metropolis
                
                
                Atrium Carceri','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/metropolis'),
 (9975,3,'Dream Chambers
                
                
                Mount Shrine &amp; Alphaxone','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/dream-chambers'),
 (9976,3,'Exo
                
                
                Tineidae','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/exo'),
 (9977,3,'Metta
                
                
                Dronny Darko &amp; ProtoU','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/metta'),
 (9978,3,'Interplanetary
                
                
                Ruptured World','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/interplanetary'),
 (9979,3,'The Old City - OST
                
                
                Atrium Carceri','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/the-old-city-ost'),
 (9980,3,'Eternal Drift
                
                
                Beyond the Ghost','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/eternal-drift'),
 (9981,3,'The Masterplan
                
                
                Dead Melodies','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/the-masterplan'),
 (9982,3,'Unheard Of
                
                
                Lesa Listvy','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/unheard-of'),
 (9983,3,'Origin
                
                
                Dronny Darko','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/origin'),
 (9984,3,'Shortwave Ruins
                
                
                Mount Shrine','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/shortwave-ruins'),
 (9985,3,'Anthropocene
                
                
                Dead Melodies &amp; Zenjungle','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/anthropocene'),
 (9986,3,'Against Civilization
                
                
                Apocryphos','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/against-civilization'),
 (9987,3,'Shadows of Forgotten Legends
                
                
                Alphaxone, ProtoU, Onasander','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/shadows-of-forgotten-legends'),
 (9988,3,'Dystopian Gate
                
                
                Alphaxone','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/dystopian-gate'),
 (9989,3,'Hastur
                
                
                Cryo Chamber Collaboration','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/hastur'),
 (9990,3,'Across the Shifting Abyss
                
                
                Dahlia&#39;s Tear','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/across-the-shifting-abyss'),
 (9991,3,'Anomalies
                
                
                ProtoU','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/anomalies'),
 (9992,3,'Black Stage of Night
                
                
                Atrium Carceri &amp; Cities Last Broadcast','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/black-stage-of-night'),
 (9993,3,'Quintessence
                
                
                Shrine','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/quintessence'),
 (9994,3,'Beringia
                
                
                Creation VI','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/beringia'),
 (9995,3,'You Disappeared
                
                
                Beyond the Ghost','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/you-disappeared'),
 (9996,3,'Memory 417
                
                
                In Quantum','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/memory-417'),
 (9997,3,'Dark Ambient of 2019','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/dark-ambient-of-2019'),
 (9998,3,'Archeoplanetary
                
                
                Ruptured World','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/archeoplanetary'),
 (9999,3,'Sector Hydra
                
                
                Dronny Darko &amp; RNGMNN','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/sector-hydra'),
 (10000,3,'The Mist Between Mirrors
                
                
                God Body Disconnect','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/the-mist-between-mirrors'),
 (10001,3,'Davidian
                
                
                Council of Nine','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/davidian'),
 (10002,3,'Transient Lunar Phenomenon
                
                
                Sphäre Sechs','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/transient-lunar-phenomenon'),
 (10003,3,'Chronosphere
                
                
                Alphaxone','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/chronosphere'),
 (10004,3,'Evangelikon
                
                
                Metatron Omega','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/evangelikon'),
 (10005,3,'Mithra
                
                
                Ager Sonus','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/mithra'),
 (10006,3,'Alive with Scars
                
                
                Flowers for Bodysnatchers','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/alive-with-scars'),
 (10007,3,'Arctic Gates
                
                
                Ugasanie &amp; Dronny Darko','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/arctic-gates'),
 (10008,3,'Ghosts on Broken Pavement
                
                
                Mount Shrine','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/ghosts-on-broken-pavement'),
 (10009,3,'Tomb of Ordeals','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/tomb-of-ordeals'),
 (10010,3,'Dark Ambient of 2018','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/dark-ambient-of-2018'),
 (10011,3,'Primal Destination
                
                
                Dead Melodies','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/primal-destination'),
 (10012,3,'Shub-Niggurath
                
                
                Cryo Chamber Collaboration','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/shub-niggurath'),
 (10013,3,'Echoes of the Future
                
                
                ProtoU','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/echoes-of-the-future'),
 (10014,3,'Furthermore
                
                
                Hilyard','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/furthermore'),
 (10015,3,'Aftermath
                
                
                Alphaxone &amp; Xerxes the Dark','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/aftermath'),
 (10016,3,'Through the Nightfall Grandeur
                
                
                Dahlia&#39;s Tear','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/through-the-nightfall-grandeur'),
 (10017,3,'Vinland
                
                
                Northumbria','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/vinland'),
 (10018,3,'Codex
                
                
                Atrium Carceri','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/codex'),
 (10019,3,'Exoplanetary
                
                
                Ruptured World','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/exoplanetary'),
 (10020,3,'Winter Restlessness
                
                
                Mount Shrine','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/winter-restlessness'),
 (10021,3,'Exit Earth
                
                
                Council of Nine','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/exit-earth'),
 (10022,3,'Abysmal
                
                
                Ugasanie &amp; Xerxes the Dark','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/abysmal'),
 (10023,3,'Edge of Solitude
                
                
                Alphaxone','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/edge-of-solitude'),
 (10024,3,'Way Home
                
                
                Lesa Listvy','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/way-home'),
 (10025,3,'The Foundations of Ruin
                
                
                Dead Melodies','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/the-foundations-of-ruin'),
 (10026,3,'Particle Void
                
                
                Sphäre Sechs','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/particle-void'),
 (10027,3,'Visitors
                
                
                EXIMIA','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/visitors'),
 (10028,3,'Black Hive
                
                
                Dronny Darko','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/black-hive'),
 (10029,3,'Ice Breath of Antarctica
                
                
                Ugasanie','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/ice-breath-of-antarctica'),
 (10030,3,'Necropolis
                
                
                Ager Sonus','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/necropolis'),
 (10031,3,'The Edge of Architecture
                
                
                ProtoU','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/the-edge-of-architecture'),
 (10032,3,'Ur Djupan Dal
                
                
                Atrium Carceri &amp; Herbst9','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/ur-djupan-dal'),
 (10033,3,'Miles to Midnight
                
                
                Atrium Carceri, Cities Last Broadcast, God Body Disconnect','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/miles-to-midnight'),
 (10034,3,'Dark Ambient of 2017','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/dark-ambient-of-2017'),
 (10035,3,'The Infinity Coordinates
                
                
                Silent Universe','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/the-infinity-coordinates'),
 (10036,3,'Heralds
                
                
                Wordclock','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/heralds'),
 (10037,3,'Asylum Beyond
                
                
                Flowers for Bodysnatchers','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/asylum-beyond'),
 (10038,3,'Yog-Sothoth
                
                
                Cryo Chamber Collaboration','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/yog-sothoth'),
 (10039,3,'Tomb of Druids
                
                
                ProtoU, Aegri Somnia, Dead Melodies, Ager Sonus, Creation VI','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/tomb-of-druids'),
 (10040,3,'Illuminatio
                
                
                Metatron Omega','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/illuminatio'),
 (10041,3,'Black Corner Den
                
                
                Atrium Carceri &amp; Cities Last Broadcast','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/black-corner-den'),
 (10042,3,'Forsaken
                
                
                Alphaxone &amp; Dronny Darko','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/forsaken'),
 (10043,3,'Promethean
                
                
                Randal Collier-Ford','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/promethean'),
 (10044,3,'Alpine Respire
                
                
                ProtoU &amp; Hilyard','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/alpine-respire'),
 (10045,3,'Trinity
                
                
                Council of Nine','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/trinity'),
 (10046,3,'Sleeper&#39;s Fate
                
                
                God Body Disconnect','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/sleepers-fate'),
 (10047,3,'Book of the Black Earth
                
                
                Ager Sonus','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/book-of-the-black-earth'),
 (10048,3,'Legends of the Wood
                
                
                Dead Melodies','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/legends-of-the-wood'),
 (10049,3,'Deus Sive Natura
                
                
                Creation VI','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/deus-sive-natura'),
 (10050,3,'Abduction
                
                
                Dronny Darko','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/abduction'),
 (10051,3,'AVA
                
                
                Keosz','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/ava'),
 (10052,3,'Markland
                
                
                Northumbria','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/markland'),
 (10053,3,'Endtime Psalms
                
                
                Aegri Somnia','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/endtime-psalms'),
 (10054,3,'The Time Machine
                
                
                SiJ','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/the-time-machine'),
 (10055,3,'Tomb of Seers
                
                
                Council of Nine, Alphaxone, Xerxes The Dark, Wolves and Horses','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/tomb-of-seers'),
 (10056,3,'Stardust
                
                
                Alphaxone &amp; ProtoU','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/stardust'),
 (10057,3,'Echo
                
                
                Apocryphos, Kammarheit, Atrium Carceri','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/echo'),
 (10058,3,'Dark Ambient of 2016','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/dark-ambient-of-2016'),
 (10059,3,'Genesis
                
                
                Paleowolf','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/genesis'),
 (10060,3,'Border of Worlds
                
                
                Ugasanie','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/border-of-worlds'),
 (10061,3,'The Hermit
                
                
                Enmarta','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/the-hermit'),
 (10062,3,'2148
                
                
                Sabled Sun','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/2148'),
 (10063,3,'Spira Igneus
                
                
                Dronny Darko','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/spira-igneus'),
 (10064,3,'Khmaoch
                
                
                ProtoU','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/khmaoch'),
 (10065,3,'Nyarlathotep
                
                
                Cryo Chamber Collaboration','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/nyarlathotep'),
 (10066,3,'Path of Dissolutions
                
                
                Hoshin','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/path-of-dissolutions'),
 (10067,3,'Love Like Blood
                
                
                Flowers for Bodysnatchers','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/love-like-blood'),
 (10068,3,'Sanctum
                
                
                Metatron Omega','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/sanctum'),
 (10069,3,'Monuments of Power
                
                
                Kolhoosi 13','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/monuments-of-power'),
 (10070,3,'Locus Arcadia
                
                
                Randal Collier-Ford, Flowers for Bodysnatchers, Council of Nine, God Body Disconnect','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/locus-arcadia'),
 (10071,3,'Stone Speak
                
                
                Apocryphos','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/stone-speak'),
 (10072,3,'The Humming Tapes
                
                
                Cities Last Broadcast','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/the-humming-tapes'),
 (10073,3,'Archives I-II
                
                
                Atrium Carceri','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/archives-i-ii'),
 (10074,3,'Lost Here
                
                
                protoU','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/lost-here'),
 (10075,3,'Echoes from Outer Silence
                
                
                Alphaxone','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/echoes-from-outer-silence'),
 (10076,3,'Reflections under the sky
                
                
                SiJ &amp; Textere Oris','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/reflections-under-the-sky'),
 (10077,3,'Dark Ambient of 2021','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/dark-ambient-of-2021'),
 (10078,3,'Neuroplasticity
                
                
                Dronny Darko','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/neuroplasticity'),
 (10079,3,'Dredge Portals
                
                
                God Body Disconnect','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/dredge-portals'),
 (10080,3,'Dark Ambient of 2015','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/dark-ambient-of-2015'),
 (10081,3,'Self Destruction Themes
                
                
                Wordclock','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/self-destruction-themes'),
 (10082,3,'Diagnosis
                
                
                Council of Nine','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/diagnosis'),
 (10083,3,'Signals IV-V-VI
                
                
                Sabled Sun','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/signals-iv-v-vi'),
 (10084,3,'Signals VI
                
                
                Sabled Sun','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/signals-vi'),
 (10085,3,'Aokigahara
                
                
                Flowers for Bodysnatchers','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/aokigahara'),
 (10086,3,'Remnants
                
                
                Randal Collier-Ford','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/remnants'),
 (10087,3,'Azathoth
                
                
                Cryo Chamber Collaboration','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/azathoth'),
 (10088,3,'Be Left to Oneself
                
                
                Keosz','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/be-left-to-oneself'),
 (10089,3,'Onyx
                
                
                Apocryphos, Kammarheit, Atrium Carceri','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/onyx'),
 (10090,3,'Eye of Tunguska
                
                
                Ugasanie','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/eye-of-tunguska'),
 (10091,3,'Absence of Motion
                
                
                Alphaxone','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/absence-of-motion'),
 (10092,3,'Signals V
                
                
                Sabled Sun','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/signals-v'),
 (10093,3,'Monde Obscure
                
                
                Aegri Somnia','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/monde-obscure'),
 (10094,3,'Earth Songs
                
                
                Dronny Darko &amp; protoU','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/earth-songs'),
 (10095,3,'Gnosis Dei
                
                
                Metatron Omega','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/gnosis-dei'),
 (10096,3,'Sea of Black
                
                
                Enmarta','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/sea-of-black'),
 (10097,3,'Dakhma
                
                
                Council of Nine','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/dakhma'),
 (10098,3,'Helluland
                
                
                Northumbria','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/helluland'),
 (10099,3,'Inner Stasis
                
                
                Cryobiosis','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/inner-stasis'),
 (10100,3,'2147
                
                
                Sabled Sun','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/2147'),
 (10101,3,'Altered Dimensions
                
                
                Alphaxone','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/altered-dimensions'),
 (10102,3,'The Architects
                
                
                Randal Collier-Ford','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/the-architects'),
 (10103,3,'Dark Ambient of 2014','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/dark-ambient-of-2014'),
 (10104,3,'Call of the North
                
                
                Ugasanie','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/call-of-the-north'),
 (10105,3,'Outer Tehom
                
                
                Dronny Darko','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/outer-tehom'),
 (10106,3,'Cthulhu
                
                
                Cryo Chamber Collaboration','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/cthulhu'),
 (10107,3,'Tomb of Empires
                
                
                Foundation Hope / Council of Nine / Alphaxone / Coph`antae Tryr','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/tomb-of-empires'),
 (10108,3,'Landscapes over the Sea
                
                
                Aveparthe','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/landscapes-over-the-sea'),
 (10109,3,'Endless
                
                
                Wordclock','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/endless'),
 (10110,3,'Signals I-III
                
                
                Sabled Sun','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/signals-i-iii'),
 (10111,3,'Signals IV
                
                
                Sabled Sun','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/signals-iv'),
 (10112,3,'Eschate Thule
                
                
                Mystified','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/eschate-thule'),
 (10113,3,'Dubbed in Black
                
                
                Alt3r3d Stat3','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/dubbed-in-black'),
 (10114,3,'Living in the Grayland
                
                
                Alphaxone','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/living-in-the-grayland'),
 (10115,3,'The Whole Path of War and Acceptance
                
                
                Halgrath','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/the-whole-path-of-war-and-acceptance'),
 (10116,3,'The Untold
                
                
                Atrium Carceri','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/the-untold'),
 (10117,3,'White Silence
                
                
                Ugasanie','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/white-silence'),
 (10118,3,'Psychosis
                
                
                Aseptic Void','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/psychosis'),
 (10119,3,'Signals III
                
                
                Sabled Sun','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/signals-iii'),
 (10120,3,'How cold is the sun
                
                
                Dark Matter','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/how-cold-is-the-sun'),
 (10121,3,'Majak
                
                
                Neizvestija','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/majak'),
 (10122,3,'Within Ruins
                
                
                Cryobiosis','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/within-ruins'),
 (10123,3,'Transmission Lost
                
                
                Sjellos','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/transmission-lost'),
 (10124,3,'The Anthropic Principle
                
                
                Anatomia De Vanitats','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/the-anthropic-principle'),
 (10125,3,'Signals II
                
                
                Sabled Sun','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/signals-ii'),
 (10126,3,'2146
                
                
                Sabled Sun','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/2146'),
 (10127,3,'Signals I
                
                
                Sabled Sun','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/signals-i'),
 (10128,3,'Compilation - Behind the Canvas of Time','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/compilation-behind-the-canvas-of-time'),
 (10129,3,'Out of Time
                
                
                Halgrath','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/out-of-time'),
 (10130,3,'Void
                
                
                Atrium Carceri','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/void'),
 (10131,3,'Reliquiae
                
                
                Atrium Carceri','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/reliquiae'),
 (10132,3,'Sacrosanct
                
                
                Atrium Carceri &amp; Eldar','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/sacrosanct'),
 (10133,3,'2145
                
                
                Sabled Sun','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/2145'),
 (10134,3,'Phrenitis
                
                
                Atrium Carceri','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/phrenitis'),
 (10135,3,'Souyuan
                
                
                Atrium Carceri','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/souyuan'),
 (10136,3,'Ptahil
                
                
                Atrium Carceri','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/ptahil'),
 (10137,3,'Kapnobatai
                
                
                Atrium Carceri','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/kapnobatai'),
 (10138,3,'Seishinbyouin
                
                
                Atrium Carceri','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/seishinbyouin'),
 (10139,3,'Cellblock
                
                
                Atrium Carceri','2022-07-20',0,'/img/0.gif','https://cryochamber.bandcamp.com/album/cellblock'),
 (10140,4,'Lichen8 aka E-Mantra - Nocturnal','2022-07-20',0,'https://f4.bcbits.com/img/a1615469273_2.jpg','https://e-mantra.bandcamp.com/album/lichen8-aka-e-mantra-nocturnal'),
 (10141,4,'E-Mantra - Acid Tomyris','2022-07-20',0,'https://f4.bcbits.com/img/a3186663374_2.jpg','https://e-mantra.bandcamp.com/album/e-mantra-acid-tomyris'),
 (10142,4,'E-Mantra - Silence 3 (24 Bits)','2022-07-20',0,'https://f4.bcbits.com/img/a2017036233_2.jpg','https://e-mantra.bandcamp.com/album/e-mantra-silence-3-24-bits'),
 (10143,4,'E-Mantra - Stapanii Timpului','2022-07-20',0,'https://f4.bcbits.com/img/a0282216124_2.jpg','https://e-mantra.bandcamp.com/album/e-mantra-stapanii-timpului'),
 (10144,4,'E-Mantra - Kaleidoscope Clouds 2 (24 bits)','2022-07-20',0,'https://f4.bcbits.com/img/a2495837348_2.jpg','https://e-mantra.bandcamp.com/track/e-mantra-kaleidoscope-clouds-2-24-bits'),
 (10145,4,'E-Mantra - Pathfinder','2022-07-20',0,'https://f4.bcbits.com/img/a3473165712_2.jpg','https://e-mantra.bandcamp.com/album/e-mantra-pathfinder'),
 (10146,4,'E-Mantra- Nemesis +Bonus Track','2022-07-20',0,'https://f4.bcbits.com/img/a4217124629_2.jpg','https://e-mantra.bandcamp.com/album/e-mantra-nemesis-bonus-track'),
 (10147,4,'E-Mantra - Arcana','2022-07-20',0,'https://f4.bcbits.com/img/a4149070760_2.jpg','https://e-mantra.bandcamp.com/album/e-mantra-arcana'),
 (10148,4,'Night Hex - Viziuni Nocturne +Bonus Track','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/album/night-hex-viziuni-nocturne-bonus-track'),
 (10149,4,'Spirit Catcher','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/album/spirit-catcher'),
 (10150,4,'E-Mantra- Retrodelika (A collection of tracks made before 2010)','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/album/e-mantra-retrodelika-a-collection-of-tracks-made-before-2010'),
 (10151,4,'E-Mantra - Last Transmission','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/album/e-mantra-last-transmission'),
 (10152,4,'E-Mantra / Kapnobatay / Night Hex','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/album/e-mantra-kapnobatay-night-hex'),
 (10153,4,'Somnium+Signals Melusine Pack','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/album/somnium-signals-melusine-pack'),
 (10154,4,'E-Mantra - Serenity 24 bits','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/album/e-mantra-serenity-24-bits'),
 (10155,4,'E-Mantra - Raining Lights 24 Bits + Bonus Track','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/album/e-mantra-raining-lights-24-bits-bonus-track'),
 (10156,4,'E-Mantra - Hermetika (24 bits)','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/album/e-mantra-hermetika-24-bits'),
 (10157,4,'E-Mantra - Silence 2 (24 Bits)','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/album/e-mantra-silence-2-24-bits'),
 (10158,4,'Kapnobatay Alias E-Mantra -Penumbra','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/album/kapnobatay-alias-e-mantra-penumbra'),
 (10159,4,'E-Mantra - Echoes From The Void Remastered','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/album/e-mantra-echoes-from-the-void-remastered'),
 (10160,4,'E​-​Mantra 15 years of Music Vol III','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/album/e-mantra-15-years-of-music-vol-iii'),
 (10161,4,'E-Mantra 15 years of Music Vol II','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/album/e-mantra-15-years-of-music-vol-ii'),
 (10162,4,'E-Mantra - 15 Years of Music Special Release Vol 1','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/album/e-mantra-15-years-of-music-special-release-vol-1'),
 (10163,4,'Kogaionon - Hyperborea','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/album/kogaionon-hyperborea'),
 (10164,4,'The Hermit&#39;s Sanctuary - 24 Bits / Remastered','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/album/the-hermits-sanctuary-24-bits-remastered'),
 (10165,4,'E-Mantra - Folding Time 24 Bits','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/album/e-mantra-folding-time-24-bits'),
 (10166,4,'E-MANTRA -Hypnos','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/album/e-mantra-hypnos'),
 (10167,4,'E-Mantra - Tartarus 24 bits','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/album/e-mantra-tartarus-24-bits'),
 (10168,4,'E-Mantra - Visions From The Past / New Mixing/ Mastering / 24 bits','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/album/e-mantra-visions-from-the-past-new-mixing-mastering-24-bits'),
 (10169,4,'E-Mantra - Silence / New Mixing/ Mastering /24 bits +Bonus track','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/album/e-mantra-silence-new-mixing-mastering-24-bits-bonus-track'),
 (10170,4,'E-Mantra - Floating Spirals','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/track/e-mantra-floating-spirals'),
 (10171,4,'E-Mantra - The singles Collection Vol I 24 bits','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/album/e-mantra-the-singles-collection-vol-i-24-bits'),
 (10172,4,'E-Mantra - Soundscapes 24 bits','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/album/e-mantra-soundscapes-24-bits'),
 (10173,4,'E-Mantra - Oblivion','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/album/e-mantra-oblivion'),
 (10174,4,'Compendium','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/album/compendium'),
 (10175,4,'Night Guardian','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/album/night-guardian'),
 (10176,4,'E-Mantra - Temple of Sun -2003','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/track/e-mantra-temple-of-sun-2003'),
 (10177,4,'E-Mantra - Soundsphere II (2003)
                
                
                E-Mantra / Mystic Mantra','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/track/e-mantra-soundsphere-ii-2003'),
 (10178,4,'E-Mantra - Liquid Frequencies (2017 Edit)','2022-07-20',0,'/img/0.gif','https://e-mantra.bandcamp.com/track/e-mantra-liquid-frequencies-2017-edit'),
 (10179,5,'Adrift on the Edge of Infinity','2022-07-20',0,'https://f4.bcbits.com/img/a1799847089_2.jpg','https://dahliastear1.bandcamp.com/album/adrift-on-the-edge-of-infinity'),
 (10180,5,'Descendants of the Moon','2022-07-20',0,'https://f4.bcbits.com/img/a1104145623_2.jpg','https://dahliastear1.bandcamp.com/album/descendants-of-the-moon-2'),
 (10181,5,'Across the Shifting Abyss','2022-07-20',0,'https://f4.bcbits.com/img/a3253282746_2.jpg','https://dahliastear1.bandcamp.com/album/across-the-shifting-abyss'),
 (10182,5,'Harmonious Euphonies For Supernatural Traumas Mesmerising Our Existences In Radient Corpuscle Galaxies - Reissue','2022-07-20',0,'https://f4.bcbits.com/img/a2446226704_2.jpg','https://dahliastear1.bandcamp.com/album/harmonious-euphonies-for-supernatural-traumas-mesmerising-our-existences-in-radient-corpuscle-galaxies-reissue'),
 (10183,5,'My Rotten Spirit Of Black - Reissue','2022-07-20',0,'https://f4.bcbits.com/img/a3972220775_2.jpg','https://dahliastear1.bandcamp.com/album/my-rotten-spirit-of-black-reissue'),
 (10184,5,'Through the Nightfall Grandeur','2022-07-20',0,'https://f4.bcbits.com/img/a0745832279_2.jpg','https://dahliastear1.bandcamp.com/album/through-the-nightfall-grandeur'),
 (10185,5,'Dreamsphere','2022-07-20',0,'/img/0.gif','https://dahliastear1.bandcamp.com/album/dreamsphere'),
 (10186,5,'Under Seven Skies','2022-07-20',0,'/img/0.gif','https://dahliastear1.bandcamp.com/album/under-seven-skies'),
 (10187,5,'My Rotten Spirit Of Black','2022-07-20',0,'/img/0.gif','https://dahliastear1.bandcamp.com/album/my-rotten-spirit-of-black'),
 (10188,5,'Harmonious Euphonies For Supernatural Traumas Mesmerising Our Existences In Radient Corpuscle Galaxies','2022-07-20',0,'/img/0.gif','https://dahliastear1.bandcamp.com/album/harmonious-euphonies-for-supernatural-traumas-mesmerising-our-existences-in-radient-corpuscle-galaxies'),
 (10189,6,'Symphonies II','2022-07-20',0,'https://f4.bcbits.com/img/a3622664403_2.jpg','https://neurotech.bandcamp.com/album/symphonies-ii'),
 (10190,6,'Deceive [Solace Bonus Track]','2022-07-20',0,'https://f4.bcbits.com/img/a2213708352_2.jpg','https://neurotech.bandcamp.com/track/deceive-solace-bonus-track'),
 (10191,6,'Solace','2022-07-20',0,'https://f4.bcbits.com/img/a3107574047_2.jpg','https://neurotech.bandcamp.com/album/solace'),
 (10192,6,'Light Betides','2022-07-20',0,'https://f4.bcbits.com/img/a4001451474_2.jpg','https://neurotech.bandcamp.com/track/light-betides-2'),
 (10193,6,'Waking Silence','2022-07-20',0,'https://f4.bcbits.com/img/a2593152013_2.jpg','https://neurotech.bandcamp.com/track/waking-silence'),
 (10194,6,'Unreleased Demos (2011 - 2016)','2022-07-20',0,'https://f4.bcbits.com/img/a0238621823_2.jpg','https://neurotech.bandcamp.com/album/unreleased-demos-2011-2016'),
 (10195,6,'The Catalyst','2022-07-20',0,'https://f4.bcbits.com/img/a2135057372_2.jpg','https://neurotech.bandcamp.com/album/the-catalyst'),
 (10196,6,'Symphonies','2022-07-20',0,'https://f4.bcbits.com/img/a3580958481_2.jpg','https://neurotech.bandcamp.com/album/symphonies'),
 (10197,6,'In Remission','2022-07-20',0,'/img/0.gif','https://neurotech.bandcamp.com/album/in-remission'),
 (10198,6,'Evasive','2022-07-20',0,'/img/0.gif','https://neurotech.bandcamp.com/album/evasive'),
 (10199,6,'Stigma','2022-07-20',0,'/img/0.gif','https://neurotech.bandcamp.com/album/stigma'),
 (10200,6,'Infra Versus Ultra','2022-07-20',0,'/img/0.gif','https://neurotech.bandcamp.com/album/infra-versus-ultra'),
 (10201,6,'The Decipher Volumes','2022-07-20',0,'/img/0.gif','https://neurotech.bandcamp.com/album/the-decipher-volumes'),
 (10202,6,'Blue Screen Planet','2022-07-20',0,'/img/0.gif','https://neurotech.bandcamp.com/album/blue-screen-planet'),
 (10203,6,'Antagonist','2022-07-20',0,'/img/0.gif','https://neurotech.bandcamp.com/album/antagonist'),
 (10204,6,'Transhuman','2022-07-20',0,'/img/0.gif','https://neurotech.bandcamp.com/album/transhuman'),
 (10205,7,'Centavra Project - Antigravity','2022-07-20',0,'https://f4.bcbits.com/img/a3492285204_2.jpg','https://globalsect.bandcamp.com/album/centavra-project-antigravity'),
 (10206,7,'VA Trinity','2022-07-20',0,'https://f4.bcbits.com/img/a2631819302_2.jpg','https://globalsect.bandcamp.com/album/va-trinity'),
 (10207,7,'Artifact303 - From The Stars','2022-07-20',0,'https://f4.bcbits.com/img/a3706703861_2.jpg','https://globalsect.bandcamp.com/album/artifact303-from-the-stars'),
 (10208,7,'Cosmic Dimension - Altar Of Wisdom
                
                
                Cosmic Dimension','2022-07-20',0,'https://f4.bcbits.com/img/a3201224439_2.jpg','https://globalsect.bandcamp.com/album/cosmic-dimension-altar-of-wisdom'),
 (10209,7,'Fiery Dawn - Lost Astronaut','2022-07-20',0,'https://f4.bcbits.com/img/a1994603396_2.jpg','https://globalsect.bandcamp.com/album/fiery-dawn-lost-astronaut'),
 (10210,7,'Globalsect Radio','2022-07-20',0,'https://f4.bcbits.com/img/a3136817191_2.jpg','https://globalsect.bandcamp.com/album/globalsect-radio'),
 (10211,7,'Atlantis - Cosmic Waves','2022-07-20',0,'https://f4.bcbits.com/img/a0417767230_2.jpg','https://globalsect.bandcamp.com/album/atlantis-cosmic-waves'),
 (10212,7,'Merrow - Odysseus
                
                
                MerrOw','2022-07-20',0,'https://f4.bcbits.com/img/a3065431633_2.jpg','https://globalsect.bandcamp.com/album/merrow-odysseus'),
 (10213,7,'VA Astronauts in The Solar System','2022-07-20',0,'/img/0.gif','https://globalsect.bandcamp.com/album/va-astronauts-in-the-solar-system'),
 (10214,7,'Katedra - We Are Not Alone','2022-07-20',0,'/img/0.gif','https://globalsect.bandcamp.com/album/katedra-we-are-not-alone'),
 (10215,7,'VA Shambhala','2022-07-20',0,'/img/0.gif','https://globalsect.bandcamp.com/album/va-shambhala'),
 (10216,7,'Median Project - In the Depths of Space','2022-07-20',0,'/img/0.gif','https://globalsect.bandcamp.com/album/median-project-in-the-depths-of-space'),
 (10217,7,'VA - Terraformer','2022-07-20',0,'/img/0.gif','https://globalsect.bandcamp.com/album/va-terraformer'),
 (10218,7,'VA The Mystery of Crystal Worlds','2022-07-20',0,'/img/0.gif','https://globalsect.bandcamp.com/album/va-the-mystery-of-crystal-worlds'),
 (10219,7,'Prologue','2022-07-20',0,'/img/0.gif','https://globalsect.bandcamp.com/album/prologue'),
 (10220,7,'Psy-H Project - Dance of Distant Worlds
                
                
                Global Sect Music','2022-07-20',0,'/img/0.gif','https://globalsect.bandcamp.com/album/psy-h-project-dance-of-distant-worlds'),
 (10221,7,'VA Space Of Power. The Legend about Great Existence of the Universe.
                
                
                Global Sect Music','2022-07-20',0,'/img/0.gif','https://globalsect.bandcamp.com/album/va-space-of-power-the-legend-about-great-existence-of-the-universe'),
 (10222,7,'The Mystery of Crystal Worlds (fantasy poem, rus)','2022-07-20',0,'/img/0.gif','https://globalsect.bandcamp.com/album/the-mystery-of-crystal-worlds-fantasy-poem-rus'),
 (10223,8,'Escape from Reality (24 Bit)
                
                
                Disconect, Cosmic Wolf','2022-07-20',0,'https://f4.bcbits.com/img/a1434698429_2.jpg','https://ionomusic.bandcamp.com/album/escape-from-reality-24-bit'),
 (10224,8,'Svita (16 Bit)
                
                
                Filip Zarkic','2022-07-20',0,'https://f4.bcbits.com/img/a0592351577_2.jpg','https://ionomusic.bandcamp.com/album/svita-16-bit'),
 (10225,8,'They Walk Among Us (24 Bit)
                
                
                Atacama','2022-07-20',0,'https://f4.bcbits.com/img/a1318780140_2.jpg','https://ionomusic.bandcamp.com/album/they-walk-among-us-24-bit'),
 (10226,8,'Olam (24 Bit)
                
                
                Stayos','2022-07-20',0,'https://f4.bcbits.com/img/a1602009130_2.jpg','https://ionomusic.bandcamp.com/album/olam-24-bit'),
 (10227,8,'Consciousmind (24 Bit)
                
                
                Elusion','2022-07-20',0,'https://f4.bcbits.com/img/a0385611532_2.jpg','https://ionomusic.bandcamp.com/album/consciousmind-24-bit'),
 (10228,8,'Choosing Thoughts (24 Bit)
                
                
                Sky Soul, Haive Music','2022-07-20',0,'https://f4.bcbits.com/img/a1053105213_2.jpg','https://ionomusic.bandcamp.com/album/choosing-thoughts-24-bit-2'),
 (10229,8,'Meditation (24 Bit)
                
                
                Shyisma','2022-07-20',0,'https://f4.bcbits.com/img/a1939941780_2.jpg','https://ionomusic.bandcamp.com/album/meditation-24-bit'),
 (10230,8,'Visual Sphere (24 Bit)
                
                
                Neutro','2022-07-20',0,'https://f4.bcbits.com/img/a2312848749_2.jpg','https://ionomusic.bandcamp.com/album/visual-sphere-24-bit'),
 (10231,8,'Another Dimensions (24 Bit)
                
                
                Britti, Improvement','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/another-dimensions-24-bit'),
 (10232,8,'Astral Silence (24 Bit)
                
                
                Aeiforos','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/astral-silence-24-bit'),
 (10233,8,'Psygressive Vol. 13
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/psygressive-vol-13'),
 (10234,8,'Contact (24 Bit)
                
                
                Expedition','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/contact-24-bit'),
 (10235,8,'Origins (24 Bit)
                
                
                Mindbenderz','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/origins-24-bit'),
 (10236,8,'Multiverse (24 Bit)
                
                
                Altered State','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/multiverse-24-bit'),
 (10237,8,'Ajna (Polygrams Remix) [24 Bit]
                
                
                One Function, Polygrams','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/ajna-polygrams-remix-24-bit'),
 (10238,8,'Mangalam (24 Bit)
                
                
                Dual Vision','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/mangalam-24-bit'),
 (10239,8,'Nocturnal (24 Bit)
                
                
                Lichen8','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/nocturnal-24-bit'),
 (10240,8,'Shiva&#39;s Mantra  (24 Bit)
                
                
                Lyktum, Rishi','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/shivas-mantra-24-bit'),
 (10241,8,'Great Beyond &quot;The Remixes&quot; (24 Bit)
                
                
                Bellatrix, SunSoul, Britti, Disconect, DX, Vlex, Aladiah, Mystika, Sabedoria','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/great-beyond-the-remixes-24-bit'),
 (10242,8,'Innervisions (24 Bit)
                
                
                Digital Symphony','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/innervisions-24-bit'),
 (10243,8,'Souful Seeds (24 Bit)
                
                
                Shabboo Harper','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/souful-seeds-24-bit'),
 (10244,8,'Sunrise Express (24 Bit)
                
                
                Osher','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sunrise-express-24-bit'),
 (10245,8,'Sun Child (24 Bit)
                
                
                SOME1','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sun-child-24-bit'),
 (10246,8,'Mauli Galaxy (24 Bit)
                
                
                Potilotti','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/mauli-galaxy-24-bit'),
 (10247,8,'Zero Gravity (24 Bit)
                
                
                Freak Control','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/zero-gravity-24-bit'),
 (10248,8,'Magnus - Transcend  (Altered State Remix) [24 Bit]
                
                
                Magnus, Altered State Remix','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/magnus-transcend-altered-state-remix-24-bit'),
 (10249,8,'Visualize &amp; Believe - Compiled by DJ Filip Landin (24 Bit)
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/visualize-believe-compiled-by-dj-filip-landin-24-bit'),
 (10250,8,'Desert (24 Bit)
                
                
                Cosmos (AR)','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/desert-24-bit'),
 (10251,8,'Underwater Desert (24 Bit)
                
                
                EAMO','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/underwater-desert-24-bit'),
 (10252,8,'Hydra X (24 Bit)
                
                
                Atacama','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/hydra-x-24-bit'),
 (10253,8,'Powehi Resurrection (24 Bit)
                
                
                Polaris (FR)','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/powehi-resurrection-24-bit'),
 (10254,8,'Inhuman (24 Bit)
                
                
                Reverse','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/inhuman-24-bit'),
 (10255,8,'Sounds Forever Lost
                
                
                Haive Music','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sounds-forever-lost'),
 (10256,8,'Spiritual Guide (24 Bit)
                
                
                Ascent, Vimana Shastra','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/spiritual-guide-24-bit'),
 (10257,8,'Xriptonity (24 Bit)
                
                
                Deep Vibration','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/xriptonity-24-bit'),
 (10258,8,'Zion (24 Bit)
                
                
                Lyktum, Alurian','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/zion-24-bit'),
 (10259,8,'Essentials
                
                
                One Function','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/essentials'),
 (10260,8,'Spectral Lines (24 Bit)
                
                
                Freak Control','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/spectral-lines-24-bit'),
 (10261,8,'Default Mode Network
                
                
                Gipsy Soul, HINAP','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/default-mode-network'),
 (10262,8,'Psygressive, Vol. 12
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/psygressive-vol-12'),
 (10263,8,'The Construction of Reality (24 Bit)
                
                
                Morrisound','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/the-construction-of-reality-24-bit'),
 (10264,8,'Erandi (24 Bit)
                
                
                Polygrams','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/erandi-24-bit-2'),
 (10265,8,'Uranus (24 Bit)
                
                
                Think Positive','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/uranus-24-bit'),
 (10266,8,'Cosmovision (24 Bit)
                
                
                Matt Jah','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/cosmovision-24-bit'),
 (10267,8,'Resolution (24 Bit)
                
                
                Aladiah','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/resolution-24-bit'),
 (10268,8,'Catisfied (24 Bit)
                
                
                Potilotti','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/catisfied-24-bit'),
 (10269,8,'Durban Poison
                
                
                Filip Zarkic &amp; Lacovetti','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/durban-poison'),
 (10270,8,'Vibration (24 Bit)
                
                
                Retronic','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/vibration-24-bit'),
 (10271,8,'Existence (24 Bit)
                
                
                Shyisma','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/existence-24-bit'),
 (10272,8,'Pacha Camaq
                
                
                Shabboo Harper','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/pacha-camaq'),
 (10273,8,'Be Free (24 Bit)
                
                
                One Function','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/be-free-24-bit-2'),
 (10274,8,'The Bass Creator 2.0 (24 Bit)
                
                
                Lyktum','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/the-bass-creator-20-24-bit'),
 (10275,8,'Message from the Sun (24 Bit)
                
                
                Sky Soul','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/message-from-the-sun-24-bit'),
 (10276,8,'Dreams (24 Bit)
                
                
                Cosmic Tone, Freak Control','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/dreams-24-bit'),
 (10277,8,'Human Code (24 Bit)
                
                
                Escape Room','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/human-code-24-bit'),
 (10278,8,'Desert Vibrations (24 Bit)
                
                
                SOME1, Cosmic Flow','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/desert-vibrations-24-bit'),
 (10279,8,'Into the Light (24 Bit)
                
                
                Disconect, Britti, Second Life','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/into-the-light-24-bit'),
 (10280,8,'One Species (24 Bit)
                
                
                Bellatrix','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/one-species-24-bit'),
 (10281,8,'Selection 2021 - Compiled by Cubixx
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/selection-2021-compiled-by-cubixx'),
 (10282,8,'Scoping in Twenty Twenty-One
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/scoping-in-twenty-twenty-one'),
 (10283,8,'Radio Galaxy  (24 Bit)
                
                
                Gipsy Soul. Dual Vision','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/radio-galaxy-24-bit'),
 (10284,8,'Under the Moon (24 Bit)
                
                
                Lyktum','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/under-the-moon-24-bit'),
 (10285,8,'May the Flow Be with You (24 Bit)
                
                
                Estefano Haze','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/may-the-flow-be-with-you-24-bit'),
 (10286,8,'Seasons
                
                
                Fusion Bass','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/seasons'),
 (10287,8,'Reformed (24 Bit)
                
                
                Reverse','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/reformed-24-bit'),
 (10288,8,'Psychedelic Dimensions - Lyktum Contest (24 Bit)
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/psychedelic-dimensions-lyktum-contest-24-bit'),
 (10289,8,'Be Present (24 Bit)
                
                
                Status Zero','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/be-present-24-bit'),
 (10290,8,'At the Hermes Side (24 Bit)
                
                
                La Claud','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/at-the-hermes-side-24-bit'),
 (10291,8,'Reality Pending
                
                
                SOME1 &amp; One Function','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/reality-pending'),
 (10292,8,'Ultraviolet (24 Bit)
                
                
                Altered State','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/ultraviolet-24-bit'),
 (10293,8,'Infinity (24 Bit)
                
                
                SunSoul','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/infinity-24-bit'),
 (10294,8,'Be Control (24 Bit)
                
                
                Shock Therapy','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/be-control-24-bit'),
 (10295,8,'Reaching Out (24 Bit)
                
                
                NEM3SI','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/reaching-out-24-bit'),
 (10296,8,'Midnight Milkshake
                
                
                Haive Music','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/midnight-milkshake'),
 (10297,8,'Vox (24 Bit)
                
                
                Formula None','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/vox-24-bit'),
 (10298,8,'Warknot (24 Bit)
                
                
                Thusgaard &amp; Bierlich','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/warknot-24-bit'),
 (10299,8,'IONOSPHERE
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/ionosphere'),
 (10300,8,'The Chronicles of Dreaming Slow
                
                
                Memorio','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/the-chronicles-of-dreaming-slow'),
 (10301,8,'The Difference (24 Bit)
                
                
                Invisible Reality, Waveform','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/the-difference-24-bit'),
 (10302,8,'Gio [24 Bit]
                
                
                Polygrams','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/gio-24-bit'),
 (10303,8,'Eternity (24 Bit)
                
                
                One Function','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/eternity-24-bit'),
 (10304,8,'Great Beyond (24 Bit)
                
                
                Bellatrix','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/great-beyond-24-bit-2'),
 (10305,8,'Made for Us (24 Bit)
                
                
                Stayos','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/made-for-us-24-bit'),
 (10306,8,'Three Fleeting Seasons
                
                
                Fusion Bass','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/three-fleeting-seasons'),
 (10307,8,'CPH4 (24 Bit)
                
                
                Cosmic Tone','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/cph4-24-bit'),
 (10308,8,'Lightworker  (24 Bit)
                
                
                Lyktum','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/lightworker-24-bit'),
 (10309,8,'Spiritual Experience (24 Bit)
                
                
                Matt Jah','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/spiritual-experience-24-bit'),
 (10310,8,'Fractal Dimension (24 Bit)
                
                
                Neutro','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/fractal-dimension-24-bit'),
 (10311,8,'New Origins (24 Bit)
                
                
                Aladiah, Numayma','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/new-origins-24-bit'),
 (10312,8,'Dual Vibrations (24 Bit)
                
                
                Deep Vibration, Dual Vision','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/dual-vibrations-24-bit'),
 (10313,8,'Connection (24 Bit)
                
                
                SOME1','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/connection-24-bit'),
 (10314,8,'Synchronicity (24 Bit)
                
                
                Cosmic Flow','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/synchronicity-24-bit'),
 (10315,8,'Perception (24 Bit)
                
                
                Altered State &amp; Xenoben','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/perception-24-bit'),
 (10316,8,'Psygressive - Vol. 11
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/psygressive-vol-11'),
 (10317,8,'Gravity Force
                
                
                Gipsy Soul, Pendulux','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/gravity-force'),
 (10318,8,'Merkaba (24 Bit)
                
                
                Shyisma','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/merkaba-24-bit'),
 (10319,8,'Dreamer (MoRsei Remix) [24 Bit]
                
                
                Lyktum, Solar Kid, MoRsei','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/dreamer-morsei-remix-24-bit'),
 (10320,8,'Euphoria (24 Bit)
                
                
                Ascent','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/euphoria-24-bit'),
 (10321,8,'Moai (24 Bit)
                
                
                Synesthetic, Analog Prog','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/moai-24-bit'),
 (10322,8,'Solar Spectrum (24 Bit)
                
                
                Eye-T','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/solar-spectrum-24-bit'),
 (10323,8,'Neural Networks (24 Bit)
                
                
                Aioaska &amp; Gipsy Soul','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/neural-networks-24-bit'),
 (10324,8,'Hologram (24 Bit)
                
                
                Mindbenderz','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/hologram-24-bit'),
 (10325,8,'The Art of Meditation (24 Bit)
                
                
                Morrisound','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/the-art-of-meditation-24-bit'),
 (10326,8,'Meditanationism (24 Bit)
                
                
                Bellatrix','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/meditanationism-24-bit'),
 (10327,8,'Monopsychism (24 Bit)
                
                
                Morego','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/monopsychism-24-bit'),
 (10328,8,'Music of the Spheres (24 Bit)
                
                
                CJ Art','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/music-of-the-spheres-24-bit'),
 (10329,8,'Yoldasin (24 Bit)
                
                
                Hinap','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/yoldasin-24-bit'),
 (10330,8,'Vortex (24 Bit)
                
                
                Reverse','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/vortex-24-bit'),
 (10331,8,'Hidden Gems (24 Bit)
                
                
                Nevalis &amp; Tippstrip','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/hidden-gems-24-bit'),
 (10332,8,'Things in the Sky (24 Bit)
                
                
                Cosmic Flow, Status Zero','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/things-in-the-sky-24-bit'),
 (10333,8,'Concept of Ganesh  (Solar Walker &amp; Espectral Remix) [24 Bit]
                
                
                Tabula Rasa (Psy)','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/concept-of-ganesh-solar-walker-espectral-remix-24-bit'),
 (10334,8,'Consequences
                
                
                NRSMN','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/consequences'),
 (10335,8,'Borealis (24 Bit)
                
                
                Ten Madison','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/borealis-24-bit-2'),
 (10336,8,'Sacred Energy (24 Bit)
                
                
                One Function','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sacred-energy-24-bit'),
 (10337,8,'Infinite Density (24 Bit)
                
                
                Aladiah','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/infinite-density-24-bit'),
 (10338,8,'Spiritual Journeys (24 Bit)
                
                
                Releasse','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/spiritual-journeys-24-bit'),
 (10339,8,'Strange Without You Here
                
                
                Shabboo Harper','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/strange-without-you-here'),
 (10340,8,'California (24 Bit)
                
                
                Escape Room','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/california-24-bit'),
 (10341,8,'Instant Summer (Suduaya Remix) [24 Bit]
                
                
                Klopfgeister, Suduaya','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/instant-summer-suduaya-remix-24-bit'),
 (10342,8,'Fixed Future (24 Bit)
                
                
                Formula None','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/fixed-future-24-bit'),
 (10343,8,'Psychedelic World (24 Bit)
                
                
                Shock Therapy','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/psychedelic-world-24-bit'),
 (10344,8,'Higher Dimensions (24 Bit)
                
                
                Bellatrix','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/higher-dimensions-24-bit'),
 (10345,8,'SOME1 - Forest Tales (24 Bit)
                
                
                SOME1','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/some1-forest-tales-24-bit'),
 (10346,8,'Sequent Sunset (24 Bit)
                
                
                Neptun 505','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sequent-sunset-24-bit'),
 (10347,8,'Space Center (24 Bit)
                
                
                Cosmic Flow, Ghost Note','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/space-center-24-bit'),
 (10348,8,'A Small Ride on the Cosmic Elevator (24 Bit)
                
                
                Forgotten Dreams','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/a-small-ride-on-the-cosmic-elevator-24-bit'),
 (10349,8,'Altered States (24 Bit)
                
                
                Polygrams','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/altered-states-24-bit'),
 (10350,8,'Desert Lotus (24 Bit)
                
                
                Aeonian (AUS), Novaforma','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/desert-lotus-24-bit'),
 (10351,8,'Beyond the Atmosphere (24 Bit)
                
                
                Cosmic Sidekick','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/beyond-the-atmosphere-24-bit'),
 (10352,8,'Source of Life (24 Bit)
                
                
                HINAP','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/source-of-life-24-bit'),
 (10353,8,'Everworld (24 Bit)
                
                
                Fertile, Numayma','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/everworld-24-bit'),
 (10354,8,'Circumstances (24 Bit)
                
                
                Deep Vibration','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/circumstances-24-bit'),
 (10355,8,'Collective Memory
                
                
                Soniq Vision','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/collective-memory'),
 (10356,8,'Devotion (Original Mix)
                
                
                Marcus (IL)','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/devotion-original-mix'),
 (10357,8,'Another Galaxy - Remix Contest (24 Bit)
                
                
                Mindbenderz','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/another-galaxy-remix-contest-24-bit'),
 (10358,8,'Serenading the Silence
                
                
                Haive Music','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/serenading-the-silence'),
 (10359,8,'5 Senses (24 Bit)
                
                
                Neutro','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/5-senses-24-bit'),
 (10360,8,'Laniakea (24 Bit)
                
                
                HINAP','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/laniakea-24-bit'),
 (10361,8,'Voices of the Ether (24 Bit)
                
                
                Diametric','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/voices-of-the-ether-24-bit'),
 (10362,8,'Consciousness (24 Bit)
                
                
                Bellatrix, Solarwalker','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/consciousness-24-bit'),
 (10363,8,'Electronic (24 Bit)
                
                
                Shyisma','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/electronic-24-bit'),
 (10364,8,'Look Ye Also While Life Lasts (24 Bit)
                
                
                Eridejah','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/look-ye-also-while-life-lasts-24-bit'),
 (10365,8,'Sky Grassers (24 Bit)
                
                
                Thusgaard &amp; Bierlich','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sky-grassers-24-bit'),
 (10366,8,'God Gaia (24 Bit)
                
                
                Ovnimoon','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/god-gaia-24-bit'),
 (10367,8,'You Must Go On (24 Bit)
                
                
                Red Sun &amp; Shade','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/you-must-go-on-24-bit'),
 (10368,8,'Principles of Unity (Doppler Remix) [24 Bit]
                
                
                One Function, Maitika, Doppler','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/principles-of-unity-doppler-remix-24-bit'),
 (10369,8,'Circle Of Life [24 Bit]
                
                
                Gipsy Soul','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/circle-of-life-24-bit'),
 (10370,8,'Blow (Cosmic Flow Remix) [24 Bit]
                
                
                Rising Dust, Cosmic Flow','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/blow-cosmic-flow-remix-24-bit'),
 (10371,8,'Primary Level (MoRsei &amp; Karmalogic Remix) [24 Bit]
                
                
                Hinap, MoRsei, Karmalogic','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/primary-level-morsei-karmalogic-remix-24-bit'),
 (10372,8,'Are You Scared of Life?
                
                
                Shabboo Harper','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/are-you-scared-of-life'),
 (10373,8,'Summoning the Ancestors feat. Manalishi [24 Bit]
                
                
                Aioaska','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/summoning-the-ancestors-feat-manalishi-24-bit'),
 (10374,8,'Hybriding Blends
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/hybriding-blends'),
 (10375,8,'Follow the Light (24 Bit)
                
                
                SOME1','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/follow-the-light-24-bit'),
 (10376,8,'Arcanoid (24 Bit)
                
                
                Escape Room','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/arcanoid-24-bit'),
 (10377,8,'Secret of Frequency (24 Bit)
                
                
                Osher','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/secret-of-frequency-24-bit'),
 (10378,8,'Visions (24 Bit)
                
                
                Tor.Ma In Dub','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/visions-24-bit'),
 (10379,8,'Enlightenment (24 Bit)
                
                
                One Function','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/enlightenment-24-bit'),
 (10380,8,'New Humanity (24 Bit)
                
                
                Hinap, Ital','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/new-humanity-24-bit'),
 (10381,8,'Psygressive, Vol.10
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/psygressive-vol-10'),
 (10382,8,'In Blast We Trust (24 Bit)
                
                
                Gipsy Soul','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/in-blast-we-trust-24-bit'),
 (10383,8,'Sacred Plants  (Zeftriax Hidra Frequency Dry Ice Remix) [24 Bit]
                
                
                Lyktum','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sacred-plants-zeftriax-hidra-frequency-dry-ice-remix-24-bit'),
 (10384,8,'5th level (Dual Vision Remix) [24 Bit]
                
                
                Mindbenderz','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/5th-level-dual-vision-remix-24-bit'),
 (10385,8,'Hp_003 (24 Bit)
                
                
                Tippstrip','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/hp-003-24-bit'),
 (10386,8,'Revival (24 Bit)
                
                
                Retronic','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/revival-24-bit'),
 (10387,8,'Circles of Life (24 Bit)
                
                
                Karmalogic','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/circles-of-life-24-bit'),
 (10388,8,'Borealis (24 Bit)
                
                
                Ten Madison','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/borealis-24-bit'),
 (10389,8,'Rise Machines (24 Bit)
                
                
                Electric Feel','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/rise-machines-24-bit'),
 (10390,8,'Chernobyl (24 Bit)
                
                
                Invisible Reality','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/chernobyl-24-bit'),
 (10391,8,'Travel in Space (Deep Vibration Remix) [24 Bit]
                
                
                Cosmic Tone, Deep Vibration','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/travel-in-space-deep-vibration-remix-24-bit'),
 (10392,8,'Red Alert (24 Bit)
                
                
                Stayos','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/red-alert-24-bit'),
 (10393,8,'Quantum Field (24 Bit)
                
                
                Mindbenderz','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/quantum-field-24-bit'),
 (10394,8,'Freezing Point
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/freezing-point'),
 (10395,8,'Selection 2020
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/selection-2020'),
 (10396,8,'Psychedelic Experience (24 Bit)
                
                
                Gipsy Soul','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/psychedelic-experience-24-bit'),
 (10397,8,'Natural History (24 Bit)
                
                
                Roger Rabbit','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/natural-history-24-bit'),
 (10398,8,'Life Engineering (24 Bit)
                
                
                Hinap','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/life-engineering-24-bit'),
 (10399,8,'Collective (24 Bit)
                
                
                Formula None','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/collective-24-bit'),
 (10400,8,'Spectral Memory (24 Bit)
                
                
                Sabretooth','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/spectral-memory-24-bit'),
 (10401,8,'Time for Freedom (24 Bit)
                
                
                Cosmic Flow, Marcus','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/time-for-freedom-24-bit'),
 (10402,8,'Delictae Vibrations (24 bit)
                
                
                SOME1','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/delictae-vibrations-24-bit'),
 (10403,8,'Extraterrestrial (24 Bit)
                
                
                Lyktum','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/extraterrestrial-24-bit'),
 (10404,8,'Equilibrium
                
                
                Aeonian','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/equilibrium'),
 (10405,8,'Intelligent Stations
                
                
                Deep Vibration (24 Bit)','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/intelligent-stations'),
 (10406,8,'Trip to Valhalla (24 Bit)
                
                
                Numayma','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/trip-to-valhalla-24-bit'),
 (10407,8,'Jungle Swing (24 Bit)
                
                
                Thusgaard &amp; Bierlich','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/jungle-swing-24-bit'),
 (10408,8,'Luster (24 Bit)
                
                
                Fertile','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/luster-24-bit'),
 (10409,8,'Inside Your Head (24 Bit)
                
                
                SOME1, One Function','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/inside-your-head-24-bit'),
 (10410,8,'Hypnosis (24 Bit)
                
                
                Neutro','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/hypnosis-24-bit'),
 (10411,8,'Mindbenderz - Human Form  (Cosmic Tone Remix) [24 Bit]
                
                
                Mindbenderz, Cosmic Tone','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/mindbenderz-human-form-cosmic-tone-remix-24-bit'),
 (10412,8,'Harmony (24 Bit)
                
                
                One Function','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/harmony-24-bit'),
 (10413,8,'Preciousness of Life (24 Bit)
                
                
                Estefano Haze','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/preciousness-of-life-24-bit'),
 (10414,8,'Emotion (24 Bit)
                
                
                Marcus (IL)','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/emotion-24-bit'),
 (10415,8,'Theory of the Unconcious
                
                
                Reverse','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/theory-of-the-unconcious'),
 (10416,8,'Mission 5D (24 Bit)
                
                
                Lyktum','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/mission-5d-24-bit'),
 (10417,8,'Disclouser (24 bit)
                
                
                Shyisma','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/disclouser-24-bit'),
 (10418,8,'Dmtv (Cosmic Flow Remix) 24bit
                
                
                Flowjob, Cosmic Flow','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/dmtv-cosmic-flow-remix-24bit'),
 (10419,8,'Open Your Eyes (24 Bit)
                
                
                One Function','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/open-your-eyes-24-bit'),
 (10420,8,'Happy Face (24 bit)
                
                
                SOME1','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/happy-face-24-bit'),
 (10421,8,'Loopy (24 Bit)
                
                
                Cosmic Tone','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/loopy-24-bit'),
 (10422,8,'Between Realities (24 Bit)
                
                
                TSP','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/between-realities-24-bit'),
 (10423,8,'World in Reverse (24 Bit)
                
                
                SUN (GR)','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/world-in-reverse-24-bit'),
 (10424,8,'Orbit (24 Bit)
                
                
                Hinap','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/orbit-24-bit'),
 (10425,8,'Beyond Your Imagination
                
                
                Sole Spirit','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/beyond-your-imagination'),
 (10426,8,'Essence of Life (24 bit)
                
                
                Freak Control','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/essence-of-life-24-bit'),
 (10427,8,'Cosmic Package
                
                
                Mindbenderz','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/cosmic-package'),
 (10428,8,'SOL Remixes (24 bit)
                
                
                Static Movement, Kalki, Vertex','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sol-remixes-24-bit'),
 (10429,8,'Subconscious (24 bit)
                
                
                Tabula Rasa (Psy)','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/subconscious-24-bit'),
 (10430,8,'Analog Collection (24 Bit)
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/analog-collection-24-bit'),
 (10431,8,'Roger Rabbit - Source Code (Darkland Remix)
                
                
                Darkland, Rogger Rabbit','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/roger-rabbit-source-code-darkland-remix'),
 (10432,8,'Summer Seeds
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/summer-seeds'),
 (10433,8,'Avant Garde
                
                
                Reverse','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/avant-garde'),
 (10434,8,'Scoping in Twenty-Twenty
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/scoping-in-twenty-twenty'),
 (10435,8,'Twisted Colors
                
                
                Osher','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/twisted-colors'),
 (10436,8,'Space Time
                
                
                Zone Tempest','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/space-time'),
 (10437,8,'How to Move a Daymoon
                
                
                Flowjob','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/how-to-move-a-daymoon'),
 (10438,8,'Twist Again
                
                
                Sundose','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/twist-again'),
 (10439,8,'Arpland
                
                
                Kettenburg','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/arpland'),
 (10440,8,'Counterpoint
                
                
                One Function &amp; Mindbenderz','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/counterpoint'),
 (10441,8,'Reflections Bundle
                
                
                SUN (GR)','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/reflections-bundle'),
 (10442,8,'Day Zero
                
                
                Xception','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/day-zero'),
 (10443,8,'Bright Spheres
                
                
                Doppler','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/bright-spheres'),
 (10444,8,'Psygressive, Vol.9
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/psygressive-vol-9'),
 (10445,8,'Let&#39;s Reconnect
                
                
                Flowjob &amp; Novotech','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/lets-reconnect'),
 (10446,8,'Futura
                
                
                Shyisma','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/futura'),
 (10447,8,'Melodica
                
                
                Stayos &amp; Shabi','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/melodica'),
 (10448,8,'Visibility (2020 Edit)
                
                
                Invisible Reality','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/visibility-2020-edit'),
 (10449,8,'Interstellar
                
                
                CJ Art','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/interstellar'),
 (10450,8,'Omega Point
                
                
                Morsei','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/omega-point'),
 (10451,8,'Consciousness
                
                
                Deep Vibration','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/consciousness'),
 (10452,8,'Explorations
                
                
                Hinap','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/explorations'),
 (10453,8,'Jupiter
                
                
                Mindbenderz','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/jupiter'),
 (10454,8,'Myself
                
                
                Shabboo Harper','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/myself'),
 (10455,8,'Yond the Virus
                
                
                Aioaska','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/yond-the-virus'),
 (10456,8,'Storyteller
                
                
                Cosmic Flow, SOME1','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/storyteller'),
 (10457,8,'Voices
                
                
                Cosmic Tone, One Function','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/voices'),
 (10458,8,'Elements
                
                
                La Claud','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/elements'),
 (10459,8,'Just Imagine
                
                
                Shock Therapy','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/just-imagine'),
 (10460,8,'Desert Storm
                
                
                SUN (GR)','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/desert-storm'),
 (10461,8,'From Within
                
                
                Formula None','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/from-within'),
 (10462,8,'Wake Us Up (Freak Control Remix)
                
                
                Cosmic Tone, Freak Control','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/wake-us-up-freak-control-remix'),
 (10463,8,'The Journey
                
                
                MoRsei &amp; Karmalogic','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/the-journey'),
 (10464,8,'Synchronicity
                
                
                Lyktum &amp; Hinap','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/synchronicity'),
 (10465,8,'Conscious Reality
                
                
                Releasse','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/conscious-reality'),
 (10466,8,'Black Express
                
                
                Georgia Luna','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/black-express'),
 (10467,8,'Sun Valley
                
                
                Sundose','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sun-valley'),
 (10468,8,'Living with Yourself
                
                
                Cylon','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/living-with-yourself'),
 (10469,8,'Galactic Scale
                
                
                Tabula Rasa (Psy) &amp; Soniq Vision','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/galactic-scale'),
 (10470,8,'Sound of Peace
                
                
                Morrisound','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sound-of-peace'),
 (10471,8,'Hidden Miracle
                
                
                Soul Frequency','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/hidden-miracle'),
 (10472,8,'Elation
                
                
                SOME1','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/elation'),
 (10473,8,'A New Paradigm
                
                
                Estefano Haze &amp; Multiphase','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/a-new-paradigm'),
 (10474,8,'Pandemaniacs
                
                
                Starseed &amp; Sensescape','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/pandemaniacs'),
 (10475,8,'Fly Mein Vogel
                
                
                Thusgaard &amp; Bierlich','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/fly-mein-vogel'),
 (10476,8,'From the Future
                
                
                Cosmic Flow &amp; Alegro','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/from-the-future'),
 (10477,8,'One Function - Ajna (IKØN Remix)
                
                
                IKØN, One Function','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/one-function-ajna-ik-n-remix'),
 (10478,8,'Thousand Winds
                
                
                Waveform','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/thousand-winds'),
 (10479,8,'Sonic Therapy
                
                
                SUN','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sonic-therapy'),
 (10480,8,'Awakening
                
                
                Osher','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/awakening'),
 (10481,8,'Solar Awakening
                
                
                Lyktum','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/solar-awakening'),
 (10482,8,'Healing Drop
                
                
                SUN feat. Sudha','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/healing-drop'),
 (10483,8,'Beatpolar
                
                
                Flowjob','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/beatpolar'),
 (10484,8,'Path of Uncertainty
                
                
                Haive Music','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/path-of-uncertainty'),
 (10485,8,'Psygressive, Vol.8
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/psygressive-vol-8'),
 (10486,8,'DreamWorld
                
                
                Warp Brothers &amp; Pablo Quinones','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/dreamworld'),
 (10487,8,'Lust in Space
                
                
                Proxius','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/lust-in-space'),
 (10488,8,'Megika
                
                
                Kettenburg','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/megika'),
 (10489,8,'Astro Light
                
                
                Marcus','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/astro-light'),
 (10490,8,'Beyond Simulation
                
                
                HINAP &amp; Reverse','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/beyond-simulation'),
 (10491,8,'Mental Universe
                
                
                Nikki S &amp; Nick Sentience','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/mental-universe'),
 (10492,8,'Reresurrection
                
                
                Cosmic Flow','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/reresurrection'),
 (10493,8,'Moonbay
                
                
                Ten Madison','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/moonbay'),
 (10494,8,'Mystic (Dual Vision Remix)
                
                
                E-Mov , Dual Vision','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/mystic-dual-vision-remix'),
 (10495,8,'Fragile Nature Remixes
                
                
                Mind &amp; Matter','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/fragile-nature-remixes'),
 (10496,8,'Invisible Reality - History Package
                
                
                Invisible Reality','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/invisible-reality-history-package'),
 (10497,8,'Stars
                
                
                Mindbenderz','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/stars'),
 (10498,8,'Infinite Gods
                
                
                Deep Vibration','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/infinite-gods'),
 (10499,8,'Essence
                
                
                Tweaken','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/essence'),
 (10500,8,'Mindless Entertainment
                
                
                SOME1','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/mindless-entertainment'),
 (10501,8,'Bewitched Desert
                
                
                Stayos &amp; Shabi','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/bewitched-desert'),
 (10502,8,'Kings Valley
                
                
                Reverse','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/kings-valley'),
 (10503,8,'Selection 2019
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/selection-2019'),
 (10504,8,'Iono Black Anthology, Archive, Vol. 2
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/iono-black-anthology-archive-vol-2'),
 (10505,8,'Refresh
                
                
                Shock Therapy','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/refresh'),
 (10506,8,'Scoping in Twenty-Nineteen
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/scoping-in-twenty-nineteen'),
 (10507,8,'Mystery
                
                
                Dual Vision &amp; Altered State','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/mystery'),
 (10508,8,'Malayalam
                
                
                Marcus','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/malayalam'),
 (10509,8,'Last Chance
                
                
                Cosmic Tone &amp; Ital','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/track/last-chance'),
 (10510,8,'Primary Level
                
                
                Hinap','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/track/primary-level'),
 (10511,8,'The Surfing Happiness
                
                
                La Claud','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/the-surfing-happiness'),
 (10512,8,'Bodyscan
                
                
                Estefano Haze &amp; Novotech','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/bodyscan'),
 (10513,8,'Creativity (Dual Vision Remix)
                
                
                Mind &amp; Matter','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/creativity-dual-vision-remix'),
 (10514,8,'Timeless Love
                
                
                Sun','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/timeless-love'),
 (10515,8,'Elevacion
                
                
                Lyktum &amp; Ital','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/elevacion'),
 (10516,8,'Psygressive, Vol.7
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/psygressive-vol-7'),
 (10517,8,'Man &amp; Nature (Spectro Senses Remix)
                
                
                Mindwave','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/man-nature-spectro-senses-remix'),
 (10518,8,'Reset
                
                
                Neptun 505','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/reset'),
 (10519,8,'What Its All About
                
                
                Photosynthesis','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/what-its-all-about'),
 (10520,8,'Big Bang
                
                
                Cosmic Tone &amp; Marcus','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/big-bang'),
 (10521,8,'The Bright Side
                
                
                Sundose','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/the-bright-side'),
 (10522,8,'Solar Plexus
                
                
                Mind &amp; Matter vs. Dual Vision','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/solar-plexus'),
 (10523,8,'Dizzy Mind
                
                
                Waveform','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/dizzy-mind'),
 (10524,8,'Seratone
                
                
                Aioaska','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/seratone'),
 (10525,8,'Perceptions
                
                
                Hinap','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/perceptions'),
 (10526,8,'Different Vibrations
                
                
                Motion Drive','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/different-vibrations'),
 (10527,8,'Northern Lights
                
                
                Freak Control','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/northern-lights'),
 (10528,8,'Human Machine
                
                
                Osher','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/human-machine-2'),
 (10529,8,'Delirium
                
                
                One Function &amp; SOME1','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/delirium'),
 (10530,8,'Borderlines &amp; Ferrytales
                
                
                Drift Away','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/borderlines-ferrytales'),
 (10531,8,'Jahe
                
                
                Marcus','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/jahe'),
 (10532,8,'Submerge
                
                
                Mindbenderz','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/submerge'),
 (10533,8,'Penumbra
                
                
                Flowjob','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/penumbra'),
 (10534,8,'Sense of Knowhere
                
                
                Sapana','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sense-of-knowhere'),
 (10535,8,'Sungazing
                
                
                Estefano Haze &amp; Multiphase','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sungazing'),
 (10536,8,'Insanity
                
                
                Sun','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/insanity'),
 (10537,8,'Psychoactive
                
                
                Cosmic Tone','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/psychoactive'),
 (10538,8,'Magic Mushrooms
                
                
                Altered State &amp; Cabal','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/magic-mushrooms'),
 (10539,8,'Chaotic Nature
                
                
                Hinap &amp; Reverse','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/chaotic-nature'),
 (10540,8,'Mind Expand
                
                
                Lyktum','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/mind-expand'),
 (10541,8,'D.N.A (Shibass Remix)
                
                
                Yahel','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/d-n-a-shibass-remix'),
 (10542,8,'Cosmic Carneval
                
                
                Aioaska feat. Uluru','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/cosmic-carneval'),
 (10543,8,'Mutualism
                
                
                Lesmus','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/mutualism'),
 (10544,8,'All at Once (Megatone Remix)
                
                
                Ritmo','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/all-at-once-megatone-remix'),
 (10545,8,'Spheres
                
                
                SOME1','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/spheres'),
 (10546,8,'Iono Music One Day in Israel!
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/iono-music-one-day-in-israel'),
 (10547,8,'Sick
                
                
                Mindbenderz','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sick'),
 (10548,8,'Mindfulness
                
                
                Lyktum','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/mindfulness'),
 (10549,8,'Day out of Time - Remixes
                
                
                Mind &amp; Matter','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/day-out-of-time-remixes'),
 (10550,8,'The Spirit
                
                
                Sundose','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/the-spirit'),
 (10551,8,'Wake Up
                
                
                Stayos','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/wake-up'),
 (10552,8,'A Kids Dream
                
                
                Hinap','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/a-kids-dream'),
 (10553,8,'Space Odyssey
                
                
                CJ Art','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/space-odyssey'),
 (10554,8,'Crystallized (Spectro Senses Remix)
                
                
                Sideform','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/crystallized-spectro-senses-remix'),
 (10555,8,'Upside Down
                
                
                Mindwave','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/upside-down'),
 (10556,8,'Holographic Matter
                
                
                Nebula Meltdown','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/holographic-matter'),
 (10557,8,'Psygressive, Vol.6
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/psygressive-vol-6'),
 (10558,8,'Waves
                
                
                AudioUnit','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/waves'),
 (10559,8,'The Message
                
                
                One Function','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/the-message'),
 (10560,8,'Reflected Solstice
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/reflected-solstice'),
 (10561,8,'Cloudsurfing
                
                
                Altered State','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/cloudsurfing'),
 (10562,8,'Abyss
                
                
                Mindbenderz','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/abyss'),
 (10563,8,'Subconscious
                
                
                Invisible Reality &amp; Waveform','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/subconscious'),
 (10564,8,'Human Nature
                
                
                Aioaska','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/human-nature'),
 (10565,8,'Midnight Drums (Outlaw Remix)
                
                
                Lyktum','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/midnight-drums-outlaw-remix'),
 (10566,8,'More Fire
                
                
                Tabula Rasa','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/more-fire'),
 (10567,8,'Baby Techno
                
                
                Vivante','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/baby-techno'),
 (10568,8,'Visions of Acting
                
                
                Hinap','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/visions-of-acting'),
 (10569,8,'Psychedelic All Over
                
                
                Osher','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/psychedelic-all-over'),
 (10570,8,'Flowing Matter
                
                
                The Bitzpan','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/flowing-matter'),
 (10571,8,'Physical Sounds
                
                
                Cosmic Tone','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/physical-sounds'),
 (10572,8,'Biorhythm
                
                
                UnderCover &amp; Samra','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/biorhythm'),
 (10573,8,'Amazonaya
                
                
                Lyktum','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/amazonaya'),
 (10574,8,'Are We Alone
                
                
                Estefano Haze','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/are-we-alone'),
 (10575,8,'Awake
                
                
                SOME1','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/awake'),
 (10576,8,'Escape
                
                
                SUN (GR)','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/escape-2'),
 (10577,8,'Mighty Jungle
                
                
                Dual Vision &amp; IKØN','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/mighty-jungle'),
 (10578,8,'Energetic Connection
                
                
                Aioaska &amp; Gipsy Soul','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/energetic-connection'),
 (10579,8,'Human Form
                
                
                Mindbenderz','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/human-form'),
 (10580,8,'Sourcecodes of Reality (IKØN Remix)
                
                
                Divination','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sourcecodes-of-reality-ik-n-remix'),
 (10581,8,'At the Beginning (Shibass Remix)
                
                
                Ritmo','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/at-the-beginning-shibass-remix'),
 (10582,8,'Flying Still
                
                
                Letissier','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/flying-still'),
 (10583,8,'Electro Panic (One Function Remix)
                
                
                Infected Mushroom &amp; Yahel','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/electro-panic-one-function-remix'),
 (10584,8,'Abduction
                
                
                Lyktum &amp; Zarma','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/abduction'),
 (10585,8,'After Life
                
                
                Photosynthesis','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/after-life'),
 (10586,8,'Escape
                
                
                Tweaken','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/escape'),
 (10587,8,'Back to the Forest
                
                
                Eridanus','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/back-to-the-forest'),
 (10588,8,'Psygressive, Vol.5
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/psygressive-vol-5'),
 (10589,8,'The Arrival
                
                
                UnderCover','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/the-arrival'),
 (10590,8,'Quantum Void
                
                
                Yestermorrow','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/quantum-void'),
 (10591,8,'Beyond
                
                
                Sky Soul','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/beyond'),
 (10592,8,'Back to My Roots (Hinap Remix)
                
                
                One Function','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/back-to-my-roots-hinap-remix'),
 (10593,8,'Extinction
                
                
                Reverse','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/extinction'),
 (10594,8,'Triangulum
                
                
                8LACKOUT','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/triangulum'),
 (10595,8,'Sacred Plants Remixes
                
                
                Lyktum','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sacred-plants-remixes'),
 (10596,8,'Melosoul
                
                
                Stayos','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/melosoul'),
 (10597,8,'Breath
                
                
                Roger Rabbit','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/breath'),
 (10598,8,'Mantra
                
                
                Indra','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/mantra'),
 (10599,8,'Go
                
                
                Lesmus','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/go'),
 (10600,8,'Visuals
                
                
                Altered State','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/visuals'),
 (10601,8,'Resources
                
                
                Cosmic Tone','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/resources'),
 (10602,8,'Time Is Unity
                
                
                Kleysky &amp; Mind Void','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/time-is-unity'),
 (10603,8,'Dragonfly
                
                
                SOME1','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/dragonfly'),
 (10604,8,'Concept of Ganesh
                
                
                Tabula Rasa','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/concept-of-ganesh'),
 (10605,8,'Sometimes
                
                
                The Bitzpan','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sometimes'),
 (10606,8,'Voice of Nature
                
                
                Ital','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/voice-of-nature'),
 (10607,8,'Tornado
                
                
                Mr.Raf &amp; Rubbi Heller','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/tornado'),
 (10608,8,'Spiritual Illumination
                
                
                Lyktum &amp; Ovnimoon','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/spiritual-illumination'),
 (10609,8,'Magical Spirit
                
                
                Mekkanikka','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/magical-spirit'),
 (10610,8,'Reality Is Now
                
                
                Hinap','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/reality-is-now'),
 (10611,8,'Selection 2018
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/selection-2018'),
 (10612,8,'Iono Black Anthology, Archive, Vol.1
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/iono-black-anthology-archive-vol-1'),
 (10613,8,'Borders of Sanity
                
                
                Sun','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/borders-of-sanity'),
 (10614,8,'Beach Atmosphere, Vol.3
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/beach-atmosphere-vol-3'),
 (10615,8,'Primordial Spiral
                
                
                Lyktum &amp; Spiritual Mode','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/primordial-spiral'),
 (10616,8,'Time Spirit
                
                
                Flexus &amp; Estefano Haze','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/time-spirit'),
 (10617,8,'Daydream
                
                
                One Function','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/daydream'),
 (10618,8,'Children of Space
                
                
                Mindwave','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/children-of-space'),
 (10619,8,'Everything Is Different
                
                
                Koi Boi','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/everything-is-different'),
 (10620,8,'Star Spirit
                
                
                Mekkanikka','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/star-spirit'),
 (10621,8,'Digital Sanskrit
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/digital-sanskrit'),
 (10622,8,'The Journey Begins
                
                
                Doppler','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/the-journey-begins'),
 (10623,8,'Illuminator
                
                
                Manmachine','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/illuminator'),
 (10624,8,'Tribalism
                
                
                Mindbenderz','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/tribalism'),
 (10625,8,'Midnight Express
                
                
                Digital Symphony','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/midnight-express'),
 (10626,8,'Mind Delights
                
                
                Sundose','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/mind-delights'),
 (10627,8,'Lost Universe
                
                
                Hujaboy &amp; Striders','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/lost-universe'),
 (10628,8,'Time After Time
                
                
                Dual Vision','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/time-after-time'),
 (10629,8,'Trip Report #01
                
                
                Tabula Rasa','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/trip-report-01'),
 (10630,8,'Neuro
                
                
                Mind &amp; Matter','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/neuro'),
 (10631,8,'Close Encounter
                
                
                Eddie Bitar','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/close-encounter'),
 (10632,8,'State of Mind
                
                
                Magnus &amp; Dimibo','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/state-of-mind'),
 (10633,8,'Inevitable
                
                
                Cosmic Tone, Static Movement &amp; Freak Control','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/inevitable'),
 (10634,8,'Shamanic Dreams
                
                
                Mindwave &amp; One Function','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/shamanic-dreams'),
 (10635,8,'Chai Chi
                
                
                Divination','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/chai-chi'),
 (10636,8,'Klopfgeist
                
                
                NEM3SI$','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/klopfgeist'),
 (10637,8,'Days of Light
                
                
                Bionix','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/days-of-light'),
 (10638,8,'Anomaly Remix Contest
                
                
                Egorythmia','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/anomaly-remix-contest'),
 (10639,8,'Psygressive, Vol.4
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/psygressive-vol-4'),
 (10640,8,'Galactic Incubation Atmospheres
                
                
                StarSeed','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/galactic-incubation-atmospheres'),
 (10641,8,'Do Your Mandala
                
                
                Freak Control','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/do-your-mandala'),
 (10642,8,'Contact the Spirit
                
                
                Dragon Culture','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/contact-the-spirit'),
 (10643,8,'Fractals
                
                
                Kanobi','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/fractals'),
 (10644,8,'Rainbow Walk
                
                
                Gipsy Soul','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/rainbow-walk'),
 (10645,8,'Synthetic Life
                
                
                Gordey Tsukanov','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/synthetic-life'),
 (10646,8,'Bleep
                
                
                Roger Rabbit','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/bleep'),
 (10647,8,'Earth Formed
                
                
                Hinap','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/earth-formed'),
 (10648,8,'10 Years of Mindwave
                
                
                Mindwave','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/10-years-of-mindwave'),
 (10649,8,'Rubix
                
                
                Altered State','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/rubix'),
 (10650,8,'Chronicles
                
                
                Yestermorrow','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/chronicles'),
 (10651,8,'Youniverse
                
                
                Sundose','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/youniverse'),
 (10652,8,'Bright to Be
                
                
                While True','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/bright-to-be'),
 (10653,8,'Nocturnal (Aioaska &amp; Gipsy Soul Remix)
                
                
                Egorythmia','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/nocturnal-aioaska-gipsy-soul-remix'),
 (10654,8,'Package
                
                
                One Function','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/package-9'),
 (10655,8,'Cosmic Ritual
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/cosmic-ritual'),
 (10656,8,'Everlasting
                
                
                Static Movement &amp; U-Recken','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/everlasting'),
 (10657,8,'Intergalactic
                
                
                Cosmic Tone','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/intergalactic'),
 (10658,8,'Nirvana Reach
                
                
                Lyktum','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/nirvana-reach'),
 (10659,8,'Chillout Lounge, Vol. 2
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/chillout-lounge-vol-2'),
 (10660,8,'R U Awake
                
                
                Mekkanikka &amp; Michele Adamson','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/r-u-awake'),
 (10661,8,'Last Mohicans
                
                
                Drukverdeler &amp; DJ Bim','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/last-mohicans'),
 (10662,8,'RMX
                
                
                Sabretooth','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/rmx-2'),
 (10663,8,'Another Deep Delta
                
                
                The Bitzpan','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/another-deep-delta'),
 (10664,8,'Scarlet
                
                
                Enlusion','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/scarlet'),
 (10665,8,'Are You Experienced
                
                
                One Function','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/are-you-experienced'),
 (10666,8,'Wormhole (Aioaska &amp; Gipsy Soul Remix)
                
                
                Mindbenderz','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/wormhole-aioaska-gipsy-soul-remix'),
 (10667,8,'Source of Energy
                
                
                SUN','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/source-of-energy'),
 (10668,8,'Titania
                
                
                Egorythmia &amp; Dual Resonance','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/titania'),
 (10669,8,'Psygressive, Vol.3
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/psygressive-vol-3'),
 (10670,8,'Cycles
                
                
                R3cycle','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/cycles'),
 (10671,8,'Shaky Memories (Static Movement &amp; Cosmic Tone Remix)
                
                
                Mad Maxx &amp; Block Device','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/shaky-memories-static-movement-cosmic-tone-remix'),
 (10672,8,'Ex-Po Zen
                
                
                Yoake','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/ex-po-zen'),
 (10673,8,'Fragile Nature
                
                
                Mind &amp; Matter','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/fragile-nature'),
 (10674,8,'Artificial Language
                
                
                Jakaan','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/artificial-language'),
 (10675,8,'Reflection (2018 Edit)
                
                
                Sideform','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/reflection-2018-edit'),
 (10676,8,'2043
                
                
                Neptun 505','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/2043'),
 (10677,8,'Creativity
                
                
                Mind &amp; Matter','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/creativity'),
 (10678,8,'Natural Response
                
                
                Aioaska &amp; Gipsy Soul','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/natural-response'),
 (10679,8,'Space Music
                
                
                Osher','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/space-music'),
 (10680,8,'Soul Particle
                
                
                Hujaboy','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/soul-particle'),
 (10681,8,'Home Is Not a Place
                
                
                Hinap','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/home-is-not-a-place'),
 (10682,8,'Maximum Overdrive (Lyktum Remix)
                
                
                Ace Ventura','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/maximum-overdrive-lyktum-remix'),
 (10683,8,'Hello R3Lebenz
                
                
                R3cycle &amp; Roy Lebens','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/hello-r3lebenz'),
 (10684,8,'Sakawaka
                
                
                Ilai &amp; Rafael Osmo','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sakawaka'),
 (10685,8,'Terra Incognita
                
                
                Bionix &amp; IKØN','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/terra-incognita'),
 (10686,8,'Galactic Ways
                
                
                Impact','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/galactic-ways'),
 (10687,8,'Forma Nuova
                
                
                Matteo Monero','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/forma-nuova'),
 (10688,8,'Nevermore
                
                
                Tristate','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/nevermore'),
 (10689,8,'One
                
                
                Mindwave','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/one-2'),
 (10690,8,'Tranceposition
                
                
                Dual Vision','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/tranceposition'),
 (10691,8,'Spirit Molecule
                
                
                Mekkanikka','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/spirit-molecule'),
 (10692,8,'Sublime
                
                
                Dezoncondor','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sublime'),
 (10693,8,'Flying High
                
                
                Manmachine','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/flying-high'),
 (10694,8,'Doppler Shift','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/doppler-shift'),
 (10695,8,'Rara Avis
                
                
                Slam Duck','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/rara-avis'),
 (10696,8,'Experiment
                
                
                Static Movement &amp; Cosmic Tone','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/experiment'),
 (10697,8,'Psygressive, Vol.2
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/psygressive-vol-2'),
 (10698,8,'Indian Spirit
                
                
                Mekkanikka &amp; Impulser','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/indian-spirit'),
 (10699,8,'Mind Fields
                
                
                Timelock &amp; Impact','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/mind-fields'),
 (10700,8,'Caveman
                
                
                Stayos','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/caveman'),
 (10701,8,'Remixes
                
                
                Hypnocoustics','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/remixes'),
 (10702,8,'Ancient Gods
                
                
                Impact &amp; Out Of Range','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/ancient-gods'),
 (10703,8,'Speed of Light
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/speed-of-light-2'),
 (10704,8,'Rising
                
                
                Anton Maiko','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/rising'),
 (10705,8,'Manali Cream
                
                
                Aioaska &amp; Gipsy Soul','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/manali-cream'),
 (10706,8,'Imaginary Fields
                
                
                Yestermorrow &amp; Mind &amp; Matter','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/imaginary-fields'),
 (10707,8,'Pure Modesty
                
                
                Gabi Peleg','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/pure-modesty'),
 (10708,8,'3 6 9
                
                
                Mindwave','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/3-6-9'),
 (10709,8,'Human
                
                
                Lyktum','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/human'),
 (10710,8,'The Last of Our Kind
                
                
                Daniel Lesden','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/the-last-of-our-kind'),
 (10711,8,'The 5th Level
                
                
                Mindbenderz','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/the-5th-level'),
 (10712,8,'Extraline
                
                
                R3cycle','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/extraline'),
 (10713,8,'Energy of Life
                
                
                Ilai &amp; One Function','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/energy-of-life'),
 (10714,8,'Sources
                
                
                Static Movement &amp; Mindwave','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sources'),
 (10715,8,'A Billion Neurons
                
                
                AudioFire (UK)','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/a-billion-neurons'),
 (10716,8,'Mandala Manoeuvres
                
                
                Sons Of Shiva','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/mandala-manoeuvres'),
 (10717,8,'V.A.Selection 2017
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/v-a-selection-2017'),
 (10718,8,'Preception of Reality
                
                
                Static Movement','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/preception-of-reality'),
 (10719,8,'Flashback
                
                
                Indra','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/flashback'),
 (10720,8,'Machina
                
                
                Lyktum','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/machina'),
 (10721,8,'Nocturnal
                
                
                Egorythmia','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/nocturnal'),
 (10722,8,'Crystal Soul
                
                
                K. I. T. T. Y','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/crystal-soul'),
 (10723,8,'Another Galaxy
                
                
                Mindbenderz','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/another-galaxy'),
 (10724,8,'Technolodic
                
                
                R3cycle','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/technolodic'),
 (10725,8,'Amusement Park
                
                
                Atomizers &amp; Yestermorrow','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/amusement-park'),
 (10726,8,'Hallucinations
                
                
                Static Movement','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/hallucinations'),
 (10727,8,'Midnight Drums
                
                
                Lyktum','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/midnight-drums'),
 (10728,8,'Symbolizes
                
                
                One Function &amp; Sonic Sense','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/symbolizes'),
 (10729,8,'ADMD
                
                
                E-Clip','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/admd'),
 (10730,8,'Endshift
                
                
                Ilai','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/endshift'),
 (10731,8,'Psygressive  Vol.1
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/psygressive-vol-1'),
 (10732,8,'Dreamland
                
                
                Static Movement','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/dreamland'),
 (10733,8,'Born Again
                
                
                One Function','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/born-again'),
 (10734,8,'Akasha
                
                
                Mindwave','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/akasha-2'),
 (10735,8,'Aum Mantra
                
                
                Manmachine','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/aum-mantra'),
 (10736,8,'All Living Beings
                
                
                Hypnocoustics &amp; Hujaboy','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/all-living-beings'),
 (10737,8,'Microdose
                
                
                Yestermorrow &amp; One Function','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/microdose'),
 (10738,8,'Forever
                
                
                Party Heroes &amp; Lost in Space','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/forever'),
 (10739,8,'Voices from Beyond
                
                
                Cathar &amp; System E','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/voices-from-beyond'),
 (10740,8,'Pulses
                
                
                Impulser','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/pulses'),
 (10741,8,'Circles of Life
                
                
                Flexus &amp; Stayos','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/circles-of-life'),
 (10742,8,'Extraterrestrials
                
                
                Static Movement','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/extraterrestrials'),
 (10743,8,'Physical Effects
                
                
                Maitika','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/physical-effects'),
 (10744,8,'Cosmic Dancer (Impact Remix)
                
                
                Mindbenderz','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/cosmic-dancer-impact-remix'),
 (10745,8,'Ajna
                
                
                One Function','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/ajna'),
 (10746,8,'Say It Louder
                
                
                Ilai &amp; Atomizers','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/say-it-louder'),
 (10747,8,'Side Dish
                
                
                Roger Rabbit','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/side-dish'),
 (10748,8,'Live, Love, Dance
                
                
                Gabi Peleg','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/live-love-dance'),
 (10749,8,'Sourcecodes of Reality
                
                
                Divination','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sourcecodes-of-reality'),
 (10750,8,'In A Dream
                
                
                Koi Boi','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/in-a-dream'),
 (10751,8,'Ancient Code
                
                
                Mindwave','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/ancient-code'),
 (10752,8,'Super Duper (Atomizers Remix)
                
                
                Avalon &amp; Waio','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/super-duper-atomizers-remix'),
 (10753,8,'Power of Celtic (Sesto Sento vs. Static Movment Remix)
                
                
                Shiva Shidapu','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/power-of-celtic-sesto-sento-vs-static-movment-remix'),
 (10754,8,'Onironauta
                
                
                Dual Vision','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/onironauta'),
 (10755,8,'Sacred Rituals
                
                
                Mindbenderz','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sacred-rituals'),
 (10756,8,'Mutation
                
                
                Egorythmia &amp; Static Movement','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/mutation'),
 (10757,8,'Gravity
                
                
                Manmachine','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/gravity'),
 (10758,8,'Boombay
                
                
                Cosmic Tone &amp; Cosmic Flow','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/boombay'),
 (10759,8,'Breaking The Silence
                
                
                One Function','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/breaking-the-silence'),
 (10760,8,'Slowing Down
                
                
                R3cycle','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/slowing-down'),
 (10761,8,'Biointegrated Design
                
                
                E-Clip','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/biointegrated-design'),
 (10762,8,'Reflexions Part One
                
                
                Protonica','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/reflexions-part-one'),
 (10763,8,'VA Destination Vol. 4
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/va-destination-vol-4'),
 (10764,8,'Liquid Spring
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/liquid-spring'),
 (10765,8,'Binary Code
                
                
                Reverse &amp; Hinap','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/binary-code'),
 (10766,8,'New Way Begins
                
                
                Static Movement &amp; Cosmic Tone','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/new-way-begins'),
 (10767,8,'We Are Universe
                
                
                Dual Resonance','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/we-are-universe'),
 (10768,8,'Reflections
                
                
                Zyrus 7','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/reflections'),
 (10769,8,'All At Once (Rocky Tilbor Remix)
                
                
                Ritmo','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/all-at-once-rocky-tilbor-remix'),
 (10770,8,'Protonica
                
                
                Protonica','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/protonica'),
 (10771,8,'The Dark And The Light
                
                
                Mindbenderz','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/the-dark-and-the-light'),
 (10772,8,'Time Dilation
                
                
                Manmachine','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/time-dilation'),
 (10773,8,'Salvia Divinorum 2017
                
                
                E-Clip','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/salvia-divinorum-2017'),
 (10774,8,'Voyager
                
                
                Mindwave','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/voyager'),
 (10775,8,'Selection 2016
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/selection-2016'),
 (10776,8,'Utopia
                
                
                Impact','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/utopia-2'),
 (10777,8,'Sadness
                
                
                Flexus','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sadness'),
 (10778,8,'The Divine Source
                
                
                Yestermorrow','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/the-divine-source'),
 (10779,8,'Apocalypto
                
                
                Space Hypnose','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/apocalypto'),
 (10780,8,'Gaya
                
                
                Stayos','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/gaya'),
 (10781,8,'One
                
                
                One Function','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/one'),
 (10782,8,'Superposition
                
                
                Sonic Entity','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/superposition'),
 (10783,8,'Vortex
                
                
                Redrosid','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/vortex'),
 (10784,8,'Unobtainium
                
                
                Inner State','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/unobtainium'),
 (10785,8,'Sian
                
                
                Static Movement','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sian'),
 (10786,8,'Dancing Stars
                
                
                Suntree','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/dancing-stars'),
 (10787,8,'Carnival Wave (Ritmo Remix)
                
                
                Rocky Tilbor','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/carnival-wave-ritmo-remix'),
 (10788,8,'Aboriginman
                
                
                Azax &amp; Pettra','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/aboriginman'),
 (10789,8,'Sophia&#39;s Culture
                
                
                Sonic Sense','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sophias-culture'),
 (10790,8,'Berzerk
                
                
                Doppler Shift &amp; Zatzak','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/berzerk'),
 (10791,8,'Principles Of Unity
                
                
                One Function &amp; Maitika','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/principles-of-unity'),
 (10792,8,'Brain Waves
                
                
                Impact','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/brain-waves'),
 (10793,8,'Northern Stars
                
                
                Static Movement &amp; Cosmic Tone','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/northern-stars'),
 (10794,8,'Wormhole
                
                
                Mindbenderz','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/wormhole'),
 (10795,8,'Source Code
                
                
                Protonica &amp; Tristate','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/source-code'),
 (10796,8,'At The Beginning (Azax Remix)
                
                
                Ritmo','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/at-the-beginning-azax-remix'),
 (10797,8,'Ayahuasca
                
                
                Side Effects &amp; Sonic Sense','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/ayahuasca'),
 (10798,8,'Package
                
                
                Suntree','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/package-8'),
 (10799,8,'Shuni-E
                
                
                Yoake','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/shuni-e'),
 (10800,8,'Spectrum
                
                
                Mindwave','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/spectrum'),
 (10801,8,'Chiki Chika
                
                
                Rocky Tilbor','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/chiki-chika'),
 (10802,8,'Soundglider (Yestermorrow Remix)
                
                
                Atmos','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/soundglider-yestermorrow-remix'),
 (10803,8,'Zero Point Field
                
                
                Atomizers','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/zero-point-field'),
 (10804,8,'Its Coming
                
                
                Static Movement &amp; Vertex','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/its-coming'),
 (10805,8,'Deep Dive
                
                
                Side Effects','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/deep-dive'),
 (10806,8,'The Future
                
                
                Invisible Reality &amp; Atacama','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/the-future-2'),
 (10807,8,'Cosmic Dancer
                
                
                Mindbenderz','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/cosmic-dancer'),
 (10808,8,'Decipher
                
                
                E-Clip','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/decipher'),
 (10809,8,'Mirror
                
                
                Skyfall','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/mirror'),
 (10810,8,'Adventures
                
                
                Ritmo','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/adventures-2'),
 (10811,8,'Nano Explosion
                
                
                Egorythmia &amp; Static Movement','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/nano-explosion'),
 (10812,8,'Cause &amp; Effect
                
                
                Inner State','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/cause-effect'),
 (10813,8,'Paradigm Shift
                
                
                Roger Rabbit','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/paradigm-shift'),
 (10814,8,'Virtual Reality
                
                
                Atomizers','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/virtual-reality'),
 (10815,8,'Project 5
                
                
                Mindwave','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/project-5'),
 (10816,8,'Space Safari
                
                
                Ilai','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/space-safari'),
 (10817,8,'Package
                
                
                Side Effects','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/package-7'),
 (10818,8,'Yantra
                
                
                One Function','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/yantra'),
 (10819,8,'Package
                
                
                Static Movement','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/package-6'),
 (10820,8,'Chill Out Realms
                
                
                Motion Drive','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/chill-out-realms'),
 (10821,8,'Reactor
                
                
                Protonica','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/reactor'),
 (10822,8,'Antimateria
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/antimateria'),
 (10823,8,'Adventures
                
                
                Ritmo','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/adventures'),
 (10824,8,'Speed Of Light
                
                
                Egorythmia &amp; Roger Rabbit','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/speed-of-light'),
 (10825,8,'Selection 2015
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/selection-2015'),
 (10826,8,'Mind Control
                
                
                Side Effects','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/mind-control'),
 (10827,8,'People Of The Light
                
                
                Yestermorrow','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/people-of-the-light'),
 (10828,8,'Mechanical Dancers
                
                
                Ilai','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/mechanical-dancers'),
 (10829,8,'Macrocosm
                
                
                E-Clip','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/macrocosm'),
 (10830,8,'Oneness
                
                
                One Function','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/oneness'),
 (10831,8,'Singularity
                
                
                Mindwave','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/singularity-2'),
 (10832,8,'Brain Signal
                
                
                Side Effects','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/brain-signal'),
 (10833,8,'Witness Me
                
                
                Redrosid','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/witness-me'),
 (10834,8,'Revolved
                
                
                Indianix','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/revolved'),
 (10835,8,'Burning Motion
                
                
                Dual Resonance','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/burning-motion'),
 (10836,8,'Drawing Electric Storms
                
                
                Ilai','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/drawing-electric-storms'),
 (10837,8,'Akasha
                
                
                Side Effects &amp; Time In Motion','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/akasha'),
 (10838,8,'Chemical Reaction
                
                
                Egorythmia &amp; Sonic Entity','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/chemical-reaction'),
 (10839,8,'Pure Reflexions
                
                
                Reverse','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/pure-reflexions'),
 (10840,8,'Package
                
                
                E-Clip','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/package-5'),
 (10841,8,'Deep &amp; Roll
                
                
                Indianix','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/deep-roll'),
 (10842,8,'Alien Product
                
                
                Lifeforms','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/alien-product'),
 (10843,8,'Matter
                
                
                Sphera','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/matter'),
 (10844,8,'Sol
                
                
                Static Movement','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sol'),
 (10845,8,'100 Billion Galaxies
                
                
                Egorythmia','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/100-billion-galaxies'),
 (10846,8,'Modus Operandi
                
                
                Inner State','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/modus-operandi'),
 (10847,8,'Shrouded
                
                
                Redrosid','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/shrouded'),
 (10848,8,'Stellar
                
                
                Mindwave','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/stellar'),
 (10849,8,'Glory
                
                
                Side Effects','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/glory'),
 (10850,8,'Rise Of The Robots
                
                
                Protonica','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/rise-of-the-robots'),
 (10851,8,'Enchantment
                
                
                E-Clip','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/enchantment'),
 (10852,8,'528hz
                
                
                Suntree','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/528hz'),
 (10853,8,'Package
                
                
                Motion Drive','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/package-4'),
 (10854,8,'Transient
                
                
                One Function','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/transient'),
 (10855,8,'Telekinesis
                
                
                Atomizers','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/telekinesis'),
 (10856,8,'Cosmic Transition
                
                
                Egorythmia &amp; Dual Resonance','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/cosmic-transition'),
 (10857,8,'Genetic Code
                
                
                Techyon','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/genetic-code'),
 (10858,8,'Fading Out
                
                
                Lifeforms','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/fading-out'),
 (10859,8,'Re-Union
                
                
                MUTe','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/re-union'),
 (10860,8,'Package
                
                
                Egorythmia','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/package-3'),
 (10861,8,'Good Times
                
                
                Sphera','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/good-times'),
 (10862,8,'Biological Rhythms
                
                
                Helber Gun','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/biological-rhythms'),
 (10863,8,'Horizon (Ticon Remix)
                
                
                Protonica','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/horizon-ticon-remix'),
 (10864,8,'The Limits Of Control
                
                
                Vertex &amp; Static Movement','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/the-limits-of-control'),
 (10865,8,'Underworld
                
                
                Invisible Reality','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/underworld'),
 (10866,8,'Illness Controls
                
                
                Redrosid','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/illness-controls'),
 (10867,8,'Space
                
                
                Mindwave','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/space'),
 (10868,8,'Frequency Vibration
                
                
                Bouncerz','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/frequency-vibration'),
 (10869,8,'Package
                
                
                Mindwave','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/package-2'),
 (10870,8,'French Plaisir Vol.3
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/french-plaisir-vol-3'),
 (10871,8,'Back To My Roots
                
                
                One Function','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/back-to-my-roots'),
 (10872,8,'Floating Joint
                
                
                Protonica','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/floating-joint'),
 (10873,8,'V.A. Selection 2014
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/v-a-selection-2014'),
 (10874,8,'Decoy
                
                
                Roger Rabbit','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/decoy'),
 (10875,8,'Package 2014
                
                
                Time In Motion','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/package-2014'),
 (10876,8,'Reunion
                
                
                Suntree feat. Sharon','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/reunion'),
 (10877,8,'Shiva Devotional
                
                
                Static Movement','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/shiva-devotional'),
 (10878,8,'Rising Hope
                
                
                Sonic Entity','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/rising-hope'),
 (10879,8,'Other Dimensions of Space
                
                
                Egorythmia &amp; Static Movement','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/other-dimensions-of-space'),
 (10880,8,'Light Around Us
                
                
                Aho','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/light-around-us'),
 (10881,8,'Freedom
                
                
                Side Effects','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/freedom'),
 (10882,8,'Brain Drain
                
                
                Opposite8','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/brain-drain'),
 (10883,8,'Package
                
                
                Ritmo','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/package'),
 (10884,8,'Inside My Head
                
                
                Paralogue','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/inside-my-head'),
 (10885,8,'Digital Jungle
                
                
                Ilai','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/digital-jungle'),
 (10886,8,'Collapse
                
                
                Redrosid','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/collapse'),
 (10887,8,'After Earth
                
                
                Space Hypnose','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/after-earth'),
 (10888,8,'Transcendence
                
                
                Motion Drive vs. Vertex','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/transcendence'),
 (10889,8,'Aether
                
                
                Mindwave','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/aether'),
 (10890,8,'Dynamic Velocities
                
                
                Static Movement &amp; Helber Gun','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/dynamic-velocities'),
 (10891,8,'Mood Swings
                
                
                Bouncerz','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/mood-swings'),
 (10892,8,'Parallel Fantasy Remixed
                
                
                Invisible Reality','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/parallel-fantasy-remixed'),
 (10893,8,'Mind Experience
                
                
                Lifeforms','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/mind-experience'),
 (10894,8,'Thoughts &amp; Ideas
                
                
                Ritmo &amp; Sphera','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/thoughts-ideas'),
 (10895,8,'Horizon
                
                
                Protonica','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/horizon'),
 (10896,8,'Human Behavior
                
                
                Inner State','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/human-behavior'),
 (10897,8,'We Share
                
                
                Atomizers','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/we-share'),
 (10898,8,'Avshi - Lemon Haze
                
                
                Avshi','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/avshi-lemon-haze'),
 (10899,8,'Into The Void
                
                
                E-Clip','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/into-the-void'),
 (10900,8,'Jungle Tribe
                
                
                Funky Dragon','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/jungle-tribe'),
 (10901,8,'Outside
                
                
                Suntree','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/outside'),
 (10902,8,'Alone In The Dark
                
                
                Sonic Sense','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/alone-in-the-dark'),
 (10903,8,'New Path
                
                
                SiLo &amp; Humerous','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/new-path'),
 (10904,8,'Lysergic
                
                
                Sonic Entity','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/lysergic'),
 (10905,8,'Life In The Universe
                
                
                Taboo','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/life-in-the-universe'),
 (10906,8,'Recharged
                
                
                Time in Motion','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/recharged'),
 (10907,8,'Singularity
                
                
                Redrosid','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/singularity'),
 (10908,8,'Distortions &amp; Simulations
                
                
                Infinity','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/distortions-simulations'),
 (10909,8,'Put Your Sunglasses On
                
                
                Static Movement','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/put-your-sunglasses-on'),
 (10910,8,'Christiania Selection Vol.3
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/christiania-selection-vol-3'),
 (10911,8,'Rush Hour
                
                
                Suntree &amp; Sphera','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/rush-hour'),
 (10912,8,'City On Mars
                
                
                Side Effects','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/city-on-mars'),
 (10913,8,'Sub Standards
                
                
                Lifeforms','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sub-standards'),
 (10914,8,'All we need
                
                
                Hi Profile','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/all-we-need'),
 (10915,8,'Animal Instincts
                
                
                Space Hypnose','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/animal-instincts'),
 (10916,8,'Man In The Moon
                
                
                Drukverdeler &amp; DJ Bim','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/man-in-the-moon'),
 (10917,8,'Neurochemistry (Yestermorrow Remix)
                
                
                Liquid Ace','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/neurochemistry-yestermorrow-remix'),
 (10918,8,'Forensic Science Vol.2
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/forensic-science-vol-2-2'),
 (10919,8,'Freefall
                
                
                Skyfall','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/freefall'),
 (10920,8,'Creation
                
                
                Invisible Reality','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/creation'),
 (10921,8,'Selection 2013
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/selection-2013'),
 (10922,8,'Flight Mode
                
                
                Opposite8','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/flight-mode'),
 (10923,8,'O-Beat
                
                
                Suntree &amp; Antigravity','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/o-beat'),
 (10924,8,'Geometrical Progressions
                
                
                Techyon','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/geometrical-progressions'),
 (10925,8,'Insight
                
                
                Mindwave','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/insight'),
 (10926,8,'Progressive Classics Vol.3
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/progressive-classics-vol-3'),
 (10927,8,'Chillout Lounge, Vol.1
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/chillout-lounge-vol-1'),
 (10928,8,'Destinations 3
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/destinations-3'),
 (10929,8,'Click &amp; Fly
                
                
                Middle Mode','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/click-fly'),
 (10930,8,'Brainstorming
                
                
                Roger Rabbit','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/brainstorming'),
 (10931,8,'Vigilante
                
                
                Redrosid','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/vigilante'),
 (10932,8,'Pump it
                
                
                Omiki','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/pump-it'),
 (10933,8,'Viewpoints
                
                
                Motion Drive','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/viewpoints'),
 (10934,8,'Whole New World
                
                
                Avshi','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/whole-new-world'),
 (10935,8,'Beach Atmosphere Vol.2
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/beach-atmosphere-vol-2'),
 (10936,8,'The Future
                
                
                Suntree','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/the-future'),
 (10937,8,'Highest Technology
                
                
                Egorythmia &amp; E-Clip','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/highest-technology'),
 (10938,8,'Reanimation
                
                
                Lifeforms','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/reanimation'),
 (10939,8,'Deep Hole - Remixed
                
                
                Darma','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/deep-hole-remixed'),
 (10940,8,'Arctic Dawn
                
                
                Egorythmia','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/arctic-dawn'),
 (10941,8,'Black Hole
                
                
                Side Effects','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/black-hole'),
 (10942,8,'Sexual Activity
                
                
                Time in Motion &amp; Flexus','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sexual-activity'),
 (10943,8,'15,000
                
                
                Ritree','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/15000'),
 (10944,8,'Vibrating
                
                
                Motion Drive vs. Flowjob','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/vibrating'),
 (10945,8,'Translucent Cells
                
                
                Helber Gun','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/translucent-cells'),
 (10946,8,'Mind Technology
                
                
                Audiotec','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/mind-technology'),
 (10947,8,'Human Machine
                
                
                Ritmo &amp; Sphera','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/human-machine'),
 (10948,8,'Cold Heart Breaker
                
                
                Infinity','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/cold-heart-breaker'),
 (10949,8,'Coded Patterns
                
                
                Loopstep','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/coded-patterns'),
 (10950,8,'Dont Stop
                
                
                Pop Art','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/dont-stop'),
 (10951,8,'Polarity
                
                
                Shyisma','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/polarity'),
 (10952,8,'Progressive Classics Vol.2
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/progressive-classics-vol-2'),
 (10953,8,'Real Faces
                
                
                Opposite8','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/real-faces'),
 (10954,8,'Drum selected
                
                
                Rocky','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/drum-selected'),
 (10955,8,'Future Evolution
                
                
                Sonic Entity','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/future-evolution'),
 (10956,8,'Phrase - B
                
                
                Ritmo','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/phrase-b'),
 (10957,8,'Illumination
                
                
                Lifeforms','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/illumination'),
 (10958,8,'The Jungle Comes Alive
                
                
                Space Hypnose','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/the-jungle-comes-alive'),
 (10959,8,'Beat Cake
                
                
                Odd Sequence','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/beat-cake'),
 (10960,8,'Spiritual System
                
                
                Subverso','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/spiritual-system'),
 (10961,8,'Trapped in a Dream
                
                
                Terahert','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/trapped-in-a-dream'),
 (10962,8,'Armada
                
                
                Darma vs. Numb','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/armada'),
 (10963,8,'Selection 2012','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/selection-2012'),
 (10964,8,'Wasted Years
                
                
                Infinity','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/wasted-years'),
 (10965,8,'Stairway to Heaven
                
                
                Sonic Sense','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/stairway-to-heaven'),
 (10966,8,'Jamaica
                
                
                Funky Dragon','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/jamaica'),
 (10967,8,'Christiania Selection Vol.2','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/christiania-selection-vol-2'),
 (10968,8,'Dance Of Shadows
                
                
                Invisible Reality vs. Space Hypnose','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/dance-of-shadows'),
 (10969,8,'One Man ́s Vision E.P.
                
                
                Side Effects','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/one-man-s-vision-e-p'),
 (10970,8,'Target Destination E.P.
                
                
                Optiloop','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/target-destination-e-p'),
 (10971,8,'Evidence E.P.
                
                
                Shyisma','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/evidence-e-p'),
 (10972,8,'Spin it Remix E.P.
                
                
                Ritmo &amp; Egorythmia','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/spin-it-remix-e-p'),
 (10973,8,'Enlightenment E.P.
                
                
                Mindwave','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/enlightenment-e-p'),
 (10974,8,'Troublemaker E.P.
                
                
                E-Clip','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/troublemaker-e-p'),
 (10975,8,'Sex Panther E.P.
                
                
                Lifeforms','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sex-panther-e-p'),
 (10976,8,'Rain or shine E.P.
                
                
                Pop Art','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/rain-or-shine-e-p'),
 (10977,8,'Progressive Classics Vol.1
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/progressive-classics-vol-1'),
 (10978,8,'Form Follows Function
                
                
                Protonica','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/form-follows-function'),
 (10979,8,'Force Fiction E.P.
                
                
                Solarix &amp; Opposite8','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/force-fiction-e-p'),
 (10980,8,'Basic Rules E.P.
                
                
                Echotek','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/basic-rules-e-p'),
 (10981,8,'Carrots &amp; Stick Vol.2','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/carrots-stick-vol-2'),
 (10982,8,'The Beginning E.P.
                
                
                Flexus','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/the-beginning-e-p'),
 (10983,8,'Energy
                
                
                Time in Motion','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/energy'),
 (10984,8,'Night Drive E.P.
                
                
                Timeless','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/night-drive-e-p'),
 (10985,8,'Alterswing E.P.
                
                
                Sonic Entity','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/alterswing-e-p'),
 (10986,8,'Vibrational Frequency E.P.
                
                
                Inner State','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/vibrational-frequency-e-p'),
 (10987,8,'Reverse E.P.
                
                
                Reverse','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/reverse-e-p-2'),
 (10988,8,'Koh Phangan E.P.
                
                
                Xshade','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/koh-phangan-e-p'),
 (10989,8,'Concept Of Freedom
                
                
                Mindwave','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/concept-of-freedom'),
 (10990,8,'Almost Everything E.P.
                
                
                Impact','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/almost-everything-e-p'),
 (10991,8,'Talking About Love E.P.
                
                
                Static Movement','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/talking-about-love-e-p'),
 (10992,8,'Modern Emotions E.P.
                
                
                Funky Dragon','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/modern-emotions-e-p'),
 (10993,8,'Complete Control E.P.
                
                
                Side Effects','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/complete-control-e-p'),
 (10994,8,'Active Power E.P.
                
                
                Shyisma','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/active-power-e-p'),
 (10995,8,'Doors of Soul
                
                
                Invisible Reality','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/doors-of-soul'),
 (10996,8,'Catalyzer E.P.
                
                
                Lyctum','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/catalyzer-e-p'),
 (10997,8,'From Another Planet E.P.
                
                
                Helber Gun','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/from-another-planet-e-p'),
 (10998,8,'Selection 2011','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/selection-2011'),
 (10999,8,'Christiania Selection','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/christiania-selection'),
 (11000,8,'Phrase - A
                
                
                Ritmo','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/phrase-a'),
 (11001,8,'Press Hold E.P.
                
                
                E-Clip','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/press-hold-e-p'),
 (11002,8,'ON / OFF - RITMO REMIX
                
                
                MUTe featuring Shay Nassi','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/on-off-ritmo-remix'),
 (11003,8,'Forensic Science','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/forensic-science'),
 (11004,8,'Next Level E.P.
                
                
                Infinity','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/next-level-e-p'),
 (11005,8,'Back in Time E.P.
                
                
                Space Hypnose','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/back-in-time-e-p'),
 (11006,8,'Recycled
                
                
                Motion Drive','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/recycled'),
 (11007,8,'Summer Selection 2011
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/summer-selection-2011'),
 (11008,8,'Smooth Vibrations
                
                
                Time in Motion &amp; Flexus','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/smooth-vibrations'),
 (11009,8,'Against Humanity E.P.
                
                
                Hi Profile','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/against-humanity-e-p'),
 (11010,8,'The Journey E.P.
                
                
                Timeless','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/the-journey-e-p'),
 (11011,8,'Fairy tale E.P.
                
                
                Static Movement','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/fairy-tale-e-p'),
 (11012,8,'VuuV Festival - 20th anniversary compilation','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/vuuv-festival-20th-anniversary-compilation'),
 (11013,8,'Looking Forward E.P.
                
                
                Liquid Space','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/looking-forward-e-p'),
 (11014,8,'The Floater Remixes E.P.
                
                
                Ritmo &amp; Rocky','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/the-floater-remixes-e-p'),
 (11015,8,'Airglow E.P.
                
                
                Unseen Dimensions','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/airglow-e-p'),
 (11016,8,'Stand up against Gravity','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/stand-up-against-gravity'),
 (11017,8,'Reverse E.P.
                
                
                Reverse','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/reverse-e-p'),
 (11018,8,'Four Degrees E.P.
                
                
                Time in Motion','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/four-degrees-e-p'),
 (11019,8,'Remixes 2011
                
                
                Egorythmia','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/remixes-2011'),
 (11020,8,'Control Group
                
                
                Infinity','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/control-group'),
 (11021,8,'First Contact E.P.
                
                
                Helber Gun','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/first-contact-e-p'),
 (11022,8,'Optimus Fault E.P.
                
                
                Loopstep','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/optimus-fault-e-p'),
 (11023,8,'Virus E.P.
                
                
                Shyisma','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/virus-e-p'),
 (11024,8,'Dark Side E.P.
                
                
                Pop Art','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/dark-side-e-p'),
 (11025,8,'Destination Unknown E.P.
                
                
                Optiloop','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/destination-unknown-e-p'),
 (11026,8,'Heavy Doses E.P.
                
                
                Major 7','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/heavy-doses-e-p'),
 (11027,8,'Human Traffic E.P.
                
                
                Mindwave','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/human-traffic-e-p'),
 (11028,8,'Parallel Fantasy
                
                
                Invisible Reality','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/parallel-fantasy'),
 (11029,8,'Terraforma
                
                
                Inner State','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/terraforma'),
 (11030,8,'Visibility Remix E.P.
                
                
                Invisible Reality','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/visibility-remix-e-p'),
 (11031,8,'Sleep Late E.P.
                
                
                Ecliptic','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sleep-late-e-p'),
 (11032,8,'Selection 2010
                
                
                Varios Artists','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/selection-2010'),
 (11033,8,'Activation
                
                
                Solano','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/activation'),
 (11034,8,'Inflame E.P.
                
                
                Funky Dragon','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/inflame-e-p'),
 (11035,8,'Step in Dive E.P.
                
                
                Loopstep','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/step-in-dive-e-p'),
 (11036,8,'Beach Atmosphere','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/beach-atmosphere'),
 (11037,8,'Don&#39;t forget E.P.
                
                
                Hi Profile','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/dont-forget-e-p'),
 (11038,8,'Digital Structures E.P.
                
                
                Terahert','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/digital-structures-e-p'),
 (11039,8,'Live a Dream E.P.
                
                
                Shyine','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/live-a-dream-e-p'),
 (11040,8,'Beyond Gravity E.P.
                
                
                Egorythmia','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/beyond-gravity-e-p'),
 (11041,8,'Refresh E.P.
                
                
                Protonica','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/refresh-e-p'),
 (11042,8,'7 Gates to Freedom E.P.
                
                
                Protoactive','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/7-gates-to-freedom-e-p'),
 (11043,8,'Full Swing','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/full-swing'),
 (11044,8,'Yellow Sunshine E.P.
                
                
                Ix Lam At','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/yellow-sunshine-e-p'),
 (11045,8,'French Plaisir 2','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/french-plaisir-2'),
 (11046,8,'ON DA WAY E.P.
                
                
                Synesthetic','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/on-da-way-e-p'),
 (11047,8,'Mechanizm - Reworks
                
                
                MUTe','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/mechanizm-reworks'),
 (11048,8,'Double Reaction E.P.
                
                
                Xshade &amp; Loopstep','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/double-reaction-e-p'),
 (11049,8,'Destruction E.P.
                
                
                Magoon','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/destruction-e-p'),
 (11050,8,'Land of 2 Suns Remix E.P.
                
                
                Aqualize','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/land-of-2-suns-remix-e-p'),
 (11051,8,'Dynamic Violence E.P.
                
                
                E-Clip &amp; Flegma','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/dynamic-violence-e-p'),
 (11052,8,'In the Spacecraft E.P.
                
                
                Terahert','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/in-the-spacecraft-e-p'),
 (11053,8,'In the dirt
                
                
                Motion Drive','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/in-the-dirt'),
 (11054,8,'Take Me Home E.P.
                
                
                N.A.S.A.','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/take-me-home-e-p'),
 (11055,8,'Voltage Control','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/voltage-control'),
 (11056,8,'Desert Nights E.P.
                
                
                MUTe','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/desert-nights-e-p'),
 (11057,8,'Puls of life E.P.
                
                
                Funky Dragon','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/puls-of-life-e-p'),
 (11058,8,'Liquid
                
                
                Float','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/liquid'),
 (11059,8,'Nose On Flyer E.P.
                
                
                Contraband','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/nose-on-flyer-e-p'),
 (11060,8,'Shumska Majka E.P.
                
                
                Hardy.Veles','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/shumska-majka-e-p'),
 (11061,8,'Expressions E.P.
                
                
                Liquid Space','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/expressions-e-p'),
 (11062,8,'Kaleidoscope E.P.
                
                
                Multiphase','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/kaleidoscope-e-p'),
 (11063,8,'Fragile Wool E.P.
                
                
                Virb &amp; Padd','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/fragile-wool-e-p'),
 (11064,8,'Electro bite
                
                
                Progenitor','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/electro-bite'),
 (11065,8,'Muzik E.P.
                
                
                Sideform','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/muzik-e-p'),
 (11066,8,'Abstract Visions','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/abstract-visions'),
 (11067,8,'Spiritual Ritual E.P.
                
                
                Invisible Reality','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/spiritual-ritual-e-p'),
 (11068,8,'Essential E.P.
                
                
                WE','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/essential-e-p'),
 (11069,8,'Unique
                
                
                N.A.S.A.','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/unique'),
 (11070,8,'Archive 9
                
                
                Ritmo','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/archive-9'),
 (11071,8,'Coconut Breeze
                
                
                TOR.MA in Dub','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/coconut-breeze'),
 (11072,8,'Perfect &amp; Perfect E.P.
                
                
                N - Levy','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/perfect-perfect-e-p'),
 (11073,8,'Outlander E.P.
                
                
                Badbug','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/outlander-e-p'),
 (11074,8,'Opposite
                
                
                Flegma &amp; Nerso','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/opposite'),
 (11075,8,'Warning E.P.
                
                
                Normalize','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/warning-e-p'),
 (11076,8,'Go Deep E.P.
                
                
                Titali','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/go-deep-e-p'),
 (11077,8,'Shear Force „The Remixes“
                
                
                Kaempfer &amp; Dietze','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/shear-force-the-remixes'),
 (11078,8,'Sunset Delights','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sunset-delights'),
 (11079,8,'New World Order E.P.
                
                
                Inner State','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/new-world-order-e-p'),
 (11080,8,'Audio Gigolo E.P.
                
                
                Time In Motion','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/audio-gigolo-e-p'),
 (11081,8,'Geometric Poetry
                
                
                Ovnimoon','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/geometric-poetry'),
 (11082,8,'Down to Earth E.P.
                
                
                Synesthetic','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/down-to-earth-e-p'),
 (11083,8,'Genetic Drift','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/genetic-drift'),
 (11084,8,'Ear Candy E.P.
                
                
                Ix Lam At','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/ear-candy-e-p'),
 (11085,8,'Pathfinder','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/pathfinder'),
 (11086,8,'Weirdo E.P.
                
                
                Solaris Vibe','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/weirdo-e-p'),
 (11087,8,'Inside
                
                
                Suntree','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/inside'),
 (11088,8,'Visibility
                
                
                Invisible Reality','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/visibility'),
 (11089,8,'Crimson Underground','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/crimson-underground'),
 (11090,8,'Beat Execute E.P.
                
                
                Egorythmia','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/beat-execute-e-p'),
 (11091,8,'Tesseract Remix E.P.
                
                
                Jaia','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/tesseract-remix-e-p'),
 (11092,8,'ON/OFF
                
                
                MUTe','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/on-off'),
 (11093,8,'Survive E.P.
                
                
                WE','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/survive-e-p'),
 (11094,8,'Carrots &amp; Stick','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/carrots-stick'),
 (11095,8,'Aqualize E.P.
                
                
                Aquafeel vs. Normalize','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/aqualize-e-p'),
 (11096,8,'Time E.P.
                
                
                Stratil','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/time-e-p'),
 (11097,8,'Simple Mutes E.P.
                
                
                Flegma','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/simple-mutes-e-p'),
 (11098,8,'Night Sight E.P.
                
                
                Egorythmia','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/night-sight-e-p'),
 (11099,8,'Private Guide E.P.
                
                
                Suntree','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/private-guide-e-p'),
 (11100,8,'Orange Wood E.P.
                
                
                Synthax','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/orange-wood-e-p'),
 (11101,8,'Beat Generation 2','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/beat-generation-2'),
 (11102,8,'Beat Generation','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/beat-generation'),
 (11103,8,'Minoru
                
                
                Minoru','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/minoru'),
 (11104,8,'Checkpoint
                
                
                Nasser','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/checkpoint'),
 (11105,8,'Tales of Fall','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/tales-of-fall'),
 (11106,8,'Destinations 2','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/destinations-2'),
 (11107,8,'Under the white Flag
                
                
                Solano','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/under-the-white-flag'),
 (11108,8,'Big Blue Story
                
                
                TOR.MA in Dub','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/big-blue-story'),
 (11109,8,'French Plaisir','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/french-plaisir'),
 (11110,8,'Disharmonic Silence
                
                
                Ritmo','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/disharmonic-silence'),
 (11111,8,'Grand Slam','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/grand-slam'),
 (11112,8,'kin ethics
                
                
                Ix Lam At','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/kin-ethics'),
 (11113,8,'Concepts
                
                
                Ailo','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/concepts'),
 (11114,8,'Travel Diaries
                
                
                Klopfgeister','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/travel-diaries'),
 (11115,8,'Dilemma
                
                
                Timedrained','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/dilemma'),
 (11116,8,'Blue Banquet','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/blue-banquet'),
 (11117,8,'Massive
                
                
                Funky Dragon','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/massive'),
 (11118,8,'Solar Signals','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/solar-signals'),
 (11119,8,'Blackout
                
                
                Solano','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/blackout'),
 (11120,8,'Destinations','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/destinations'),
 (11121,8,'Sunflavoured','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/sunflavoured'),
 (11122,8,'Back from Beyond','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/back-from-beyond'),
 (11123,8,'Evolutyon
                
                
                Koaxkaos','2022-07-20',0,'/img/0.gif','https://ionomusic.bandcamp.com/album/evolutyon'),
 (11124,9,'Gloria In Excelsis Lumen
                
                
                Nebula Meltdown','2022-07-20',0,'https://f4.bcbits.com/img/a0936753998_2.jpg','https://suntriprecords.bandcamp.com/album/gloria-in-excelsis-lumen'),
 (11125,9,'Live With The Lag
                
                
                Filteria','2022-07-20',0,'https://f4.bcbits.com/img/a0638511545_2.jpg','https://suntriprecords.bandcamp.com/album/live-with-the-lag'),
 (11126,9,'Retrospecter
                
                
                Various Tim Schuldt projects','2022-07-20',0,'https://f4.bcbits.com/img/a1603067301_2.jpg','https://suntriprecords.bandcamp.com/album/retrospecter'),
 (11127,9,'Songs From A Forgotten Memory
                
                
                Battle of the Future Buddhas','2022-07-20',0,'https://f4.bcbits.com/img/a1834898663_2.jpg','https://suntriprecords.bandcamp.com/album/songs-from-a-forgotten-memory'),
 (11128,9,'Eternal Freedom
                
                
                Afgin','2022-07-20',0,'https://f4.bcbits.com/img/a3986150209_2.jpg','https://suntriprecords.bandcamp.com/album/eternal-freedom'),
 (11129,9,'Suntopia
                
                
                Various Artists','2022-07-20',0,'https://f4.bcbits.com/img/a0789293803_2.jpg','https://suntriprecords.bandcamp.com/album/suntopia'),
 (11130,9,'Another Galaxy
                
                
                Median Project','2022-07-20',0,'https://f4.bcbits.com/img/a0707903648_2.jpg','https://suntriprecords.bandcamp.com/album/another-galaxy'),
 (11131,9,'Etamines
                
                
                Khetzal','2022-07-20',0,'https://f4.bcbits.com/img/a1684472904_2.jpg','https://suntriprecords.bandcamp.com/album/etamines-3'),
 (11132,9,'Carpe Noctem
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/carpe-noctem'),
 (11133,9,'Mental Triplex - Beyond
                
                
                Mindsphere','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/mental-triplex-beyond'),
 (11134,9,'Gamma Draconis
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/gamma-draconis'),
 (11135,9,'Human Control
                
                
                Triquetra','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/human-control'),
 (11136,9,'Kretsløp
                
                
                Clementz','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/kretsl-p'),
 (11137,9,'Beyond the Wormhole
                
                
                Hypnoxock','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/beyond-the-wormhole'),
 (11138,9,'Pure Energy
                
                
                MFG','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/pure-energy'),
 (11139,9,'Incandescent
                
                
                Celestial Intelligence','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/incandescent'),
 (11140,9,'Perspective
                
                
                Sykespico','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/perspective'),
 (11141,9,'Battle of the Future Buddhas - The Light Behind The Sun','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/battle-of-the-future-buddhas-the-light-behind-the-sun'),
 (11142,9,'Median Project - Constellation
                
                
                Median Project','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/median-project-constellation'),
 (11143,9,'Tokyo Live 1998
                
                
                Total Eclipse','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/tokyo-live-1998'),
 (11144,9,'Psylent Buddhi - Secrets of the Atom','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/psylent-buddhi-secrets-of-the-atom'),
 (11145,9,'The 50th Parallel
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/the-50th-parallel'),
 (11146,9,'Brain Chemistry
                
                
                Denshi Danshi','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/brain-chemistry'),
 (11147,9,'Stapanii Timpului
                
                
                E-Mantra','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/stapanii-timpului'),
 (11148,9,'Morphic Resonance - Perplexity
                
                
                Morphic Resonance','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/morphic-resonance-perplexity'),
 (11149,9,'Mystique Of The Metaverse
                
                
                Ray Castle &amp; Collaborators','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/mystique-of-the-metaverse'),
 (11150,9,'Triquetra - Ecstatic Planet
                
                
                Triquetra','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/triquetra-ecstatic-planet'),
 (11151,9,'Astral Projection - Let There Be Light
                
                
                Astral Projection','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/astral-projection-let-there-be-light'),
 (11152,9,'Identity Mash
                
                
                K.O.B.','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/identity-mash'),
 (11153,9,'VA - Inti','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/va-inti'),
 (11154,9,'Mindsphere - Mental Triplex : Mindream
                
                
                Mindsphere','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/mindsphere-mental-triplex-mindream'),
 (11155,9,'Imba - First Encounter','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/imba-first-encounter'),
 (11156,9,'Beyond Duality
                
                
                Crossing Mind','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/beyond-duality'),
 (11157,9,'Ra - Earthcall
                
                
                Ra','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/ra-earthcall'),
 (11158,9,'Live at 10 Years Suntrip Records by Fractal Gate - April 30th 2014
                
                
                Crossing Mind','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/live-at-10-years-suntrip-records-by-fractal-gate-april-30th-2014'),
 (11159,9,'Morphic Resonance - The City of Moons','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/morphic-resonance-the-city-of-moons'),
 (11160,9,'Synaptic Electrophoresis
                
                
                Toï Doï','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/synaptic-electrophoresis'),
 (11161,9,'Mindsphere - Mental Triplex : Presence
                
                
                Mindsphere','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/mindsphere-mental-triplex-presence'),
 (11162,9,'Night Hex - Viziuni Nocturne
                
                
                Night Hex','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/night-hex-viziuni-nocturne'),
 (11163,9,'Fluid Dynamics (流体動力)
                
                
                Denshi Danshi','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/fluid-dynamics'),
 (11164,9,'VA - Aurora Sidera','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/va-aurora-sidera'),
 (11165,9,'Asia 2001 - Psykadelia
                
                
                Asia 2001','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/asia-2001-psykadelia'),
 (11166,9,'Prana - Remixes 2015
                
                
                Prana','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/prana-remixes-2015'),
 (11167,9,'Perpetual Energy
                
                
                Celestial Intelligence','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/perpetual-energy'),
 (11168,9,'Retroscape
                
                
                Shakta','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/retroscape'),
 (11169,9,'Amphibians on Spacedock
                
                
                Goasia','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/amphibians-on-spacedock'),
 (11170,9,'Epoch of the Terrans
                
                
                VA','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/epoch-of-the-terrans'),
 (11171,9,'VA - Ten Spins Around the Sun','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/va-ten-spins-around-the-sun'),
 (11172,9,'E-Mantra - Nemesis
                
                
                E-Mantra','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/e-mantra-nemesis'),
 (11173,9,'Cosmic Dimension - In A Special Kind of Space
                
                
                Cosmic Dimension','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/cosmic-dimension-in-a-special-kind-of-space'),
 (11174,9,'Nebula Meltdown - Stardust Chronicles
                
                
                Nebula Meltdown','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/nebula-meltdown-stardust-chronicles'),
 (11175,9,'VA - Blacklight Moments','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/va-blacklight-moments'),
 (11176,9,'Dimension 5 - TransStellar','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/dimension-5-transstellar'),
 (11177,9,'Dimension 5 - TransAddendum','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/dimension-5-transaddendum'),
 (11178,9,'Mindsphere - Patience for Heaven
                
                
                Mindsphere','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/mindsphere-patience-for-heaven'),
 (11179,9,'InnerSpace - InnerSpace
                
                
                InnerSpace','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/innerspace-innerspace'),
 (11180,9,'Crossing Mind - The Inner Shift
                
                
                Crossing Mind','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/crossing-mind-the-inner-shift'),
 (11181,9,'VA - Shaltu','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/va-shaltu-2'),
 (11182,9,'E-Mantra - Pathfinder
                
                
                E-Mantra','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/e-mantra-pathfinder'),
 (11183,9,'Artifact303 - Back to Space
                
                
                Artifact303','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/artifact303-back-to-space'),
 (11184,9,'Antares - Exodus
                
                
                Antares','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/antares-exodus'),
 (11185,9,'Electrypnose - Sweet Sadness
                
                
                Electrypnose','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/electrypnose-sweet-sadness'),
 (11186,9,'VA - Temple of Chaos','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/va-temple-of-chaos'),
 (11187,9,'VA - Energy Waves','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/va-energy-waves'),
 (11188,9,'E-Mantra - Arcana
                
                
                E-Mantra','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/e-mantra-arcana'),
 (11189,9,'Afgin - Astral Experience
                
                
                Afgin','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/afgin-astral-experience'),
 (11190,9,'Merr0w - Born Underwater
                
                
                Merr0w','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/merr0w-born-underwater'),
 (11191,9,'VA - Sundrops','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/va-sundrops'),
 (11192,9,'Ra - 9th','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/ra-9th'),
 (11193,9,'VA - Opus Iridium','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/va-opus-iridium'),
 (11194,9,'Goasia - From Other Spaces','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/goasia-from-other-spaces'),
 (11195,9,'Dimension 5 - Transdimensional
                
                
                Dimension 5','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/dimension-5-transdimensional'),
 (11196,9,'VA - Twist Dreams','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/va-twist-dreams'),
 (11197,9,'Ka-Sol - Fairytale
                
                
                Ka-Sol','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/ka-sol-fairytale'),
 (11198,9,'Khetzal - Corolle
                
                
                Khetzal','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/khetzal-corolle'),
 (11199,9,'VA - Apsara','2022-07-20',0,'/img/0.gif','https://suntriprecords.bandcamp.com/album/va-apsara'),
 (11200,10,'Sintese - Rainbow Aura (ovniLP964 - Ovnimoon Records)','2022-07-20',0,'https://f4.bcbits.com/img/a3588142061_2.jpg','https://ovnimoonrecords.bandcamp.com/album/sintese-rainbow-aura-ovnilp964-ovnimoon-records'),
 (11201,10,'Sharmatix - Outerversal Realities (ovniep504 - Ovnimoon Records)','2022-07-20',0,'https://f4.bcbits.com/img/a2718332086_2.jpg','https://ovnimoonrecords.bandcamp.com/album/sharmatix-outerversal-realities-ovniep504-ovnimoon-records'),
 (11202,10,'ovniep503-Seismic (BR) - Nature&#39;s Intelligence (ovniep503 - Ovnimoon Records)','2022-07-20',0,'https://f4.bcbits.com/img/a3561759260_2.jpg','https://ovnimoonrecords.bandcamp.com/album/ovniep503-seismic-br-natures-intelligence-ovniep503-ovnimoon-records'),
 (11203,10,'Sintese - Figment (ovniLP963 - Ovnimoon Records)','2022-07-20',0,'https://f4.bcbits.com/img/a1165370521_2.jpg','https://ovnimoonrecords.bandcamp.com/album/sintese-figment-ovnilp963-ovnimoon-records'),
 (11204,10,'Singular Engine - Terra (ovniep502 - Ovnimoon Records)','2022-07-20',0,'https://f4.bcbits.com/img/a2374604581_2.jpg','https://ovnimoonrecords.bandcamp.com/album/singular-engine-terra-ovniep502-ovnimoon-records'),
 (11205,10,'Molecular Synthesis - Compiled by Mutana Kataro (ovniLP960 - Ovnimoon Records)','2022-07-20',0,'https://f4.bcbits.com/img/a1948182139_2.jpg','https://ovnimoonrecords.bandcamp.com/album/molecular-synthesis-compiled-by-mutana-kataro-ovnilp960-ovnimoon-records'),
 (11206,10,'World Of Psytrance 9 (Compiled By Ovnimoon) (ovniLP962 - Ovnimoon Records)','2022-07-20',0,'https://f4.bcbits.com/img/a1371527928_2.jpg','https://ovnimoonrecords.bandcamp.com/album/world-of-psytrance-9-compiled-by-ovnimoon-ovnilp962-ovnimoon-records'),
 (11207,10,'Sci-Flyers, Psychicnova - Lucid Explorers (ovniep501 - Ovnimoon Records)','2022-07-20',0,'https://f4.bcbits.com/img/a2506434564_2.jpg','https://ovnimoonrecords.bandcamp.com/album/sci-flyers-psychicnova-lucid-explorers-ovniep501-ovnimoon-records'),
 (11208,10,'Focus Mind - Infinity Universe (ovniep497 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/focus-mind-infinity-universe-ovniep497-ovnimoon-records'),
 (11209,10,'Sudamerica Psytrance (Compiled By Ovnimoon) (ovniLP961 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sudamerica-psytrance-compiled-by-ovnimoon-ovnilp961-ovnimoon-records'),
 (11210,10,'Ascent - Mind Reader (The Key Mix) (ovniep499 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ascent-mind-reader-the-key-mix-ovniep499-ovnimoon-records'),
 (11211,10,'Sectastral - Evolutionary Descendant (ovniep498 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sectastral-evolutionary-descendant-ovniep498-ovnimoon-records'),
 (11212,10,'Atmospherika - Vida (ovniep496 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/atmospherika-vida-ovniep496-ovnimoon-records'),
 (11213,10,'World Of Psytrance 8 (Compiled by Aurawave)(ovniLP959 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/world-of-psytrance-8-compiled-by-aurawave-ovnilp959-ovnimoon-records'),
 (11214,10,'Pezze -The 4th Dimension (ovniep495 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/pezze-the-4th-dimension-ovniep495-ovnimoon-records'),
 (11215,10,'Sintese - Ambient Fluids (ovniLP958 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sintese-ambient-fluids-ovnilp958-ovnimoon-records'),
 (11216,10,'Atomas 303 - When Stars Align (ovniep494 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/atomas-303-when-stars-align-ovniep494-ovnimoon-records'),
 (11217,10,'Deadhand - Trascend Reality (ovniep491 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/deadhand-trascend-reality-ovniep491-ovnimoon-records'),
 (11218,10,'Etal - Spacetime (ovniep493 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/etal-spacetime-ovniep493-ovnimoon-records'),
 (11219,10,'PsySequenz - Weird Dimension (ovniep492 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/psysequenz-weird-dimension-ovniep492-ovnimoon-records'),
 (11220,10,'Sharmatix - Fractal Experience (ovniep489 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sharmatix-fractal-experience-ovniep489-ovnimoon-records'),
 (11221,10,'World Of Psychill 2 (Ambient Downtempo Ethno Beats Selected By Barby) (ovniLP955 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/world-of-psychill-2-ambient-downtempo-ethno-beats-selected-by-barby-ovnilp955-ovnimoon-records'),
 (11222,10,'GOAstral - Astral Connections (ovniep488 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/goastral-astral-connections-ovniep488-ovnimoon-records'),
 (11223,10,'Sintese - Music (ovniLP956 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sintese-music-ovnilp956-ovnimoon-records'),
 (11224,10,'Chile Psytrance 3 (Selected By Ovnimoon) (ovniLP953 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/chile-psytrance-3-selected-by-ovnimoon-ovnilp953-ovnimoon-records'),
 (11225,10,'Maxmar - Espada de Plata (ovniep483 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/maxmar-espada-de-plata-ovniep483-ovnimoon-records'),
 (11226,10,'Singular Engine - EBE (ovniep478 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/singular-engine-ebe-ovniep478-ovnimoon-records'),
 (11227,10,'Sectastral, Vimana Shastra, Vicky Merlino - Escape Reality (ovniep479 - Ovnimoon Records','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sectastral-vimana-shastra-vicky-merlino-escape-reality-ovniep479-ovnimoon-records'),
 (11228,10,'World Of Psytrance 7 (Compiled By Atomas 303) (ovniLP952 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/world-of-psytrance-7-compiled-by-atomas-303-ovnilp952-ovnimoon-records'),
 (11229,10,'Barby - Angel Droid (ovniLP957 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/barby-angel-droid-ovnilp957-ovnimoon-records'),
 (11230,10,'Inner Engineers - In Here Is The Dream (ovniep487 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/inner-engineers-in-here-is-the-dream-ovniep487-ovnimoon-records'),
 (11231,10,'Ovnimoon, Pezze - Light Language Song (ovniep486 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ovnimoon-pezze-light-language-song-ovniep486-ovnimoon-records'),
 (11232,10,'World Of Psytrance 6 (Compiled By GOAstral) (ovniLP950 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/world-of-psytrance-6-compiled-by-goastral-ovnilp950-ovnimoon-records'),
 (11233,10,'PatchBay &amp; Puzzle - Compass To Nature (ovniep484 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/patchbay-puzzle-compass-to-nature-ovniep484-ovnimoon-records'),
 (11234,10,'Sintese - Infinitude (ovniLP947 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sintese-infinitude-ovnilp947-ovnimoon-records'),
 (11235,10,'Aurawave - Sea Room Transmission (ovniep480 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/aurawave-sea-room-transmission-ovniep480-ovnimoon-records'),
 (11236,10,'World Of Psychill (Ambient Downtempo Ethno Beats Selected By Ovnimoon) (ovniLP954 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/world-of-psychill-ambient-downtempo-ethno-beats-selected-by-ovnimoon-ovnilp954-ovnimoon-records'),
 (11237,10,'High Q - Space Paranoids (ovniep482 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/high-q-space-paranoids-ovniep482-ovnimoon-records'),
 (11238,10,'Solar Based - Holistic Materia (ovniep475 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/solar-based-holistic-materia-ovniep475-ovnimoon-records'),
 (11239,10,'Atomas 303 - In The Dark (ovniep474 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/atomas-303-in-the-dark-ovniep474-ovnimoon-records'),
 (11240,10,'Pelotero - Infinity (ovniep481 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/pelotero-infinity-ovniep481-ovnimoon-records'),
 (11241,10,'Protonix - Dream (ovniep473 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/protonix-dream-ovniep473-ovnimoon-records'),
 (11242,10,'Singular Engine - Resolar (ovniep477 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/singular-engine-resolar-ovniep477-ovnimoon-records'),
 (11243,10,'Kezo Moon - Sony Venture (ovniep472 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/kezo-moon-sony-venture-ovniep472-ovnimoon-records'),
 (11244,10,'Ovnimoon, Barby And Feel - Love Inside (ovniep476 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ovnimoon-barby-and-feel-love-inside-ovniep476-ovnimoon-records'),
 (11245,10,'Drollkoppz - E-Live (ovniep471 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/drollkoppz-e-live-ovniep471-ovnimoon-records'),
 (11246,10,'Sintese - Here On Earth (ovniLP951 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sintese-here-on-earth-ovnilp951-ovnimoon-records'),
 (11247,10,'Trizono - Shiva Me (ovniep470 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/trizono-shiva-me-ovniep470-ovnimoon-records'),
 (11248,10,'World Of Psytrance 5 (Compiled By At Mind) (ovniLP949 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/world-of-psytrance-5-compiled-by-at-mind-ovnilp949-ovnimoon-records'),
 (11249,10,'Coredata - Majora (ovniep469 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/coredata-majora-ovniep469-ovnimoon-records'),
 (11250,10,'Etal - Transition (ovniLP948 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/etal-transition-ovnilp948-ovnimoon-records'),
 (11251,10,'Sintese - Life To Come (ovniLP945 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sintese-life-to-come-ovnilp945-ovnimoon-records'),
 (11252,10,'Second Side - The Infinity Of Human Stupidity (ovniep468 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/second-side-the-infinity-of-human-stupidity-ovniep468-ovnimoon-records'),
 (11253,10,'At MinD - Out Of That Space (ovniep467 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/at-mind-out-of-that-space-ovniep467-ovnimoon-records'),
 (11254,10,'Sintese - Pacifier (ovniLP944 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sintese-pacifier-ovnilp944-ovnimoon-records'),
 (11255,10,'Humanology - The Second Lesson (ovniep466 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/humanology-the-second-lesson-ovniep466-ovnimoon-records'),
 (11256,10,'Barby - Spectre (ovniLP946 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/barby-spectre-ovnilp946-ovnimoon-records'),
 (11257,10,'At MinD - Mystery (ovniep465 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/at-mind-mystery-ovniep465-ovnimoon-records'),
 (11258,10,'Sintese - Echolocation (ovniLP943 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sintese-echolocation-ovnilp943-ovnimoon-records'),
 (11259,10,'Ovnimoon &amp; Pezze - Alma (ovniep464 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ovnimoon-pezze-alma-ovniep464-ovnimoon-records'),
 (11260,10,'Sun Melody - Shanti (ovniep463 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sun-melody-shanti-ovniep463-ovnimoon-records'),
 (11261,10,'Pelotero - Monsters (ovniep462 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/pelotero-monsters-ovniep462-ovnimoon-records'),
 (11262,10,'Feel - Avalon (ovniep461 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/feel-avalon-ovniep461-ovnimoon-records'),
 (11263,10,'Tomasian - Tribal Party (ovniep460 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/tomasian-tribal-party-ovniep460-ovnimoon-records'),
 (11264,10,'Deep Creation, Atomic Henzai - Mission Aborted (ovniep459 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/deep-creation-atomic-henzai-mission-aborted-ovniep459-ovnimoon-records'),
 (11265,10,'The Digital Blonde vs Tandy - Antithesis (Ovnimoon Edit) (ovniep458 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/the-digital-blonde-vs-tandy-antithesis-ovnimoon-edit-ovniep458-ovnimoon-records'),
 (11266,10,'Space Cream &amp; Uyazi Zulu - Macumba (ovniep457 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/space-cream-uyazi-zulu-macumba-ovniep457-ovnimoon-records'),
 (11267,10,'Le Guide - Full Ohm (ovniep456 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/le-guide-full-ohm-ovniep456-ovnimoon-records'),
 (11268,10,'Arkadia - Ravelution (ovniep455 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/arkadia-ravelution-ovniep455-ovnimoon-records'),
 (11269,10,'Sharmatix - Oracle Of Yin (ovniep454 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sharmatix-oracle-of-yin-ovniep454-ovnimoon-records'),
 (11270,10,'PriestCT - Game Over (ovniep453 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/priestct-game-over-ovniep453-ovnimoon-records'),
 (11271,10,'Morrisound - Eye Of The Beholder (Atongmu Remix) (ovniep450 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/morrisound-eye-of-the-beholder-atongmu-remix-ovniep450-ovnimoon-records'),
 (11272,10,'Ovnimoon, Pezze - Light Language Song (ovniep452 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ovnimoon-pezze-light-language-song-ovniep452-ovnimoon-records'),
 (11273,10,'Aura Borealis - Brainsugar (ovniLP942 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/aura-borealis-brainsugar-ovnilp942-ovnimoon-records'),
 (11274,10,'Yondo And Lorca &amp; Chinguaga - Organic Tea (ovniep451 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/yondo-and-lorca-chinguaga-organic-tea-ovniep451-ovnimoon-records'),
 (11275,10,'Atomas 303 - Enlightenment (ovniep449 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/atomas-303-enlightenment-ovniep449-ovnimoon-records'),
 (11276,10,'At Mind - Signs (ovniep448 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/at-mind-signs-ovniep448-ovnimoon-records'),
 (11277,10,'Sintese - Meridians (ovniLP941 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sintese-meridians-ovnilp941-ovnimoon-records'),
 (11278,10,'Rush - Dream Essence (ovniep447 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/rush-dream-essence-ovniep447-ovnimoon-records'),
 (11279,10,'Singular Engine - Natajara (ovniep446 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/singular-engine-natajara-ovniep446-ovnimoon-records'),
 (11280,10,'District Solaris, Wolfboy - Dystopia (ovniep445 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/district-solaris-wolfboy-dystopia-ovniep445-ovnimoon-records'),
 (11281,10,'Atomas 303, Left4Deaf - 2000 Kilometers To The Stars (ovniep444 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/atomas-303-left4deaf-2000-kilometers-to-the-stars-ovniep444-ovnimoon-records'),
 (11282,10,'Knu - Psyritual Illusion (ovniep443 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/knu-psyritual-illusion-ovniep443-ovnimoon-records'),
 (11283,10,'Pura Vibe - Pulse (ovniep442 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/pura-vibe-pulse-ovniep442-ovnimoon-records'),
 (11284,10,'Dekin - Revolution (ovniep441 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/dekin-revolution-ovniep441-ovnimoon-records'),
 (11285,10,'District Solaris - Psychedlic State (ovniep440 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/district-solaris-psychedlic-state-ovniep440-ovnimoon-records'),
 (11286,10,'World Of Psytrance 4 (ovniLP939 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/world-of-psytrance-4-ovnilp939-ovnimoon-records'),
 (11287,10,'Bitbox - Control Voltage (ovniep439 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/bitbox-control-voltage-ovniep439-ovnimoon-records'),
 (11288,10,'World Of Psytrance 3 (ovniLP938 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/world-of-psytrance-3-ovnilp938-ovnimoon-records'),
 (11289,10,'Aurawave - Energy Injection (ovniep438 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/aurawave-energy-injection-ovniep438-ovnimoon-records'),
 (11290,10,'World Of Psytrance 2 (ovniLP937 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/world-of-psytrance-2-ovnilp937-ovnimoon-records'),
 (11291,10,'Aureal Number - Senales (ovniLP940 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/aureal-number-senales-ovnilp940-ovnimoon-records'),
 (11292,10,'World Of Psytrance (ovniLP936 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/world-of-psytrance-ovnilp936-ovnimoon-records'),
 (11293,10,'Prosonik - Dream Om (ovniep437 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/prosonik-dream-om-ovniep437-ovnimoon-records'),
 (11294,10,'Morrisound - Remains  (ovniep436 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/morrisound-remains-ovniep436-ovnimoon-records'),
 (11295,10,'Zinuru - Evolution (ovniep435 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/zinuru-evolution-ovniep435-ovnimoon-records'),
 (11296,10,'Larbaceo - Metamorfosis (ovniep434 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/larbaceo-metamorfosis-ovniep434-ovnimoon-records'),
 (11297,10,'Ananda Shanti - In Love With The Source (ovniep433 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ananda-shanti-in-love-with-the-source-ovniep433-ovnimoon-records'),
 (11298,10,'Mysteries of Psytrance v8 (ovnicd131 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/mysteries-of-psytrance-v8-ovnicd131-ovnimoon-records'),
 (11299,10,'Eliphas - Neverending Salvation (ovniLP935 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/eliphas-neverending-salvation-ovnilp935-ovnimoon-records'),
 (11300,10,'Atomas 303 - Oobleck (Inner Engineers Remix) (ovniep432 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/atomas-303-oobleck-inner-engineers-remix-ovniep432-ovnimoon-records'),
 (11301,10,'Adapa - Banana Monkeys (ovniep431 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/adapa-banana-monkeys-ovniep431-ovnimoon-records'),
 (11302,10,'Heart Of Goa v7 (ovnicd130 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/heart-of-goa-v7-ovnicd130-ovnimoon-records'),
 (11303,10,'Geomevtrix - Alien Movement (ovniep430 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/geomevtrix-alien-movement-ovniep430-ovnimoon-records'),
 (11304,10,'Trillian - The Source (ovniep429 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/trillian-the-source-ovniep429-ovnimoon-records'),
 (11305,10,'Washuma - Chuncho (ovniep428 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/washuma-chuncho-ovniep428-ovnimoon-records'),
 (11306,10,'Golden Drop - We Are The Future (ovniep427 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/golden-drop-we-are-the-future-ovniep427-ovnimoon-records'),
 (11307,10,'Claudio Arditti - Terra (ovniLP934 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/claudio-arditti-terra-ovnilp934-ovnimoon-records'),
 (11308,10,'Active Limbic System - Chaos  (ovniep426 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/active-limbic-system-chaos-ovniep426-ovnimoon-records'),
 (11309,10,'Barby - Jaguar (ovniep425 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/barby-jaguar-ovniep425-ovnimoon-records'),
 (11310,10,'Elepho - The Greatest Of Mysteries (Redit 2020) (ovniep424 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/elepho-the-greatest-of-mysteries-redit-2020-ovniep424-ovnimoon-records'),
 (11311,10,'Quantik - Natural Energy (ovniep423 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/quantik-natural-energy-ovniep423-ovnimoon-records'),
 (11312,10,'Ohrusvuda - Steps (ovniLP933 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ohrusvuda-steps-ovnilp933-ovnimoon-records'),
 (11313,10,'BeedKraft - Syntax (ovniep422 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/beedkraft-syntax-ovniep422-ovnimoon-records'),
 (11314,10,'Aurawave - Hyperspace (ovniep421 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/aurawave-hyperspace-ovniep421-ovnimoon-records'),
 (11315,10,'Elegy - Inside (The Remixes) (ovniLP932 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/elegy-inside-the-remixes-ovnilp932-ovnimoon-records'),
 (11316,10,'Improvement - Acid Sensation (ovniep420 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/improvement-acid-sensation-ovniep420-ovnimoon-records'),
 (11317,10,'Microlin - That No Man Take Thy Crown (ovniep415 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/microlin-that-no-man-take-thy-crown-ovniep415-ovnimoon-records'),
 (11318,10,'Mictlan - Quetzalcoatl (ovniep419 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/mictlan-quetzalcoatl-ovniep419-ovnimoon-records'),
 (11319,10,'Disia - AlgoRhytmic (ovniep418 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/disia-algorhytmic-ovniep418-ovnimoon-records'),
 (11320,10,'Electit - New Civilisation (ovniep417 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/electit-new-civilisation-ovniep417-ovnimoon-records'),
 (11321,10,'ViskoPlastik - Below Zero (ovniep416 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/viskoplastik-below-zero-ovniep416-ovnimoon-records'),
 (11322,10,'Marshmalien - Synesthesia (ovniep414 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/marshmalien-synesthesia-ovniep414-ovnimoon-records'),
 (11323,10,'Median Project - Subsconscious (ovniep413 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/median-project-subsconscious-ovniep413-ovnimoon-records'),
 (11324,10,'Tobeats - Wavenoise (ovniep412 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/tobeats-wavenoise-ovniep412-ovnimoon-records'),
 (11325,10,'Goastral - Oryba Astral (ovniep411 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/goastral-oryba-astral-ovniep411-ovnimoon-records'),
 (11326,10,'Bent - Whim (ovniep410 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/bent-whim-ovniep410-ovnimoon-records'),
 (11327,10,'Horizom - Evolve Or Repeat (ovniep409 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/horizom-evolve-or-repeat-ovniep409-ovnimoon-records'),
 (11328,10,'Rush - Star Dust (ovniep408 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/rush-star-dust-ovniep408-ovnimoon-records'),
 (11329,10,'Dezoncondor - Find Your Extension (ovniep407- Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/dezoncondor-find-your-extension-ovniep407-ovnimoon-records'),
 (11330,10,'Natural Life Essence - Forms Of Life (Sonosphere Remixes)  (ovniep406- Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/natural-life-essence-forms-of-life-sonosphere-remixes-ovniep406-ovnimoon-records'),
 (11331,10,'Nintu - Tiamat (ovniep405- Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/nintu-tiamat-ovniep405-ovnimoon-records'),
 (11332,10,'Analog Minds - Phase (ovniep404- Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/analog-minds-phase-ovniep404-ovnimoon-records'),
 (11333,10,'Studiofreakz, Shark3y - Urban Forest (ovniep403- Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/studiofreakz-shark3y-urban-forest-ovniep403-ovnimoon-records'),
 (11334,10,'Syntax Error, Nanospace - 2023 (ovniep402- Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/syntax-error-nanospace-2023-ovniep402-ovnimoon-records'),
 (11335,10,'Prematron, Pavaka - The Om Element (ovniep401- Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/prematron-pavaka-the-om-element-ovniep401-ovnimoon-records'),
 (11336,10,'Aurawave - Oscillating Particles (ovniep400- Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/aurawave-oscillating-particles-ovniep400-ovnimoon-records'),
 (11337,10,'Mana Shield - Mental Discipline (ovniep398- Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/mana-shield-mental-discipline-ovniep398-ovnimoon-records'),
 (11338,10,'Memorio - Ember Reincarnate (ovniep397- Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/memorio-ember-reincarnate-ovniep397-ovnimoon-records'),
 (11339,10,'Cosmic Light - Harmony (ovniep396- Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/cosmic-light-harmony-ovniep396-ovnimoon-records'),
 (11340,10,'Mind Evolution - Harmonic Mentality (ovniep395- Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/mind-evolution-harmonic-mentality-ovniep395-ovnimoon-records'),
 (11341,10,'Electit - Different Reality (ovniep393- Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/electit-different-reality-ovniep393-ovnimoon-records'),
 (11342,10,'Alcyon - Stillness Against Movement (ovniep393- Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/alcyon-stillness-against-movement-ovniep393-ovnimoon-records'),
 (11343,10,'The Trancemancer - Extraterrestrial (ovniep392- Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/the-trancemancer-extraterrestrial-ovniep392-ovnimoon-records'),
 (11344,10,'Dekin, Mind Evolution - Damodaraya (ovniep391- Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/dekin-mind-evolution-damodaraya-ovniep391-ovnimoon-records'),
 (11345,10,'Rave[n] - Melancholica (ovniep390- Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/rave-n-melancholica-ovniep390-ovnimoon-records'),
 (11346,10,'Men in Space - Beware Of The Borg (ovniep389- Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/men-in-space-beware-of-the-borg-ovniep389-ovnimoon-records'),
 (11347,10,'Morrisound - Krishna (ovniep388- Ovnimoon Records)
                
                
                Morrisound','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/morrisound-krishna-ovniep388-ovnimoon-records'),
 (11348,10,'Koyote - Darkness  (ovniep387- Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/koyote-darkness-ovniep387-ovnimoon-records'),
 (11349,10,'Psybuddy - Mantra Mahakala (ovniep386 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/psybuddy-mantra-mahakala-ovniep386-ovnimoon-records'),
 (11350,10,'Morrisound - Time (ovniep385 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/morrisound-time-ovniep385-ovnimoon-records'),
 (11351,10,'Cosmic Light - Harmony  (ovniep383 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/cosmic-light-harmony-ovniep383-ovnimoon-records'),
 (11352,10,'Norma Project - Magical Land -Dekin, Cosmic Light (Remix) (ovniep384 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/norma-project-magical-land-dekin-cosmic-light-remix-ovniep384-ovnimoon-records'),
 (11353,10,'Manjushri - Inner World (ovniep382 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/manjushri-inner-world-ovniep382-ovnimoon-records'),
 (11354,10,'Mana Shield -Sequence Of Dreams (ovniep381 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/mana-shield-sequence-of-dreams-ovniep381-ovnimoon-records'),
 (11355,10,'Karmalogic - The Labyrinth (ovniep377- Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/karmalogic-the-labyrinth-ovniep377-ovnimoon-records'),
 (11356,10,'Kalaedo - Ending Credit  (ovniep379 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/kalaedo-ending-credit-ovniep379-ovnimoon-records'),
 (11357,10,'Progressive Goa &amp; Psy Trance Wizards 2020 Top 10 Hits, Vol1','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/progressive-goa-psy-trance-wizards-2020-top-10-hits-vol1'),
 (11358,10,'Progressive Psychedelic Trance Spotlight 2020 Top 20 Hits, Vol1','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/progressive-psychedelic-trance-spotlight-2020-top-20-hits-vol1'),
 (11359,10,'Psy Trance &amp; Goa Memories 2020 Top 20 Hits, Vol1','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/psy-trance-goa-memories-2020-top-20-hits-vol1'),
 (11360,10,'Psychedelic Trance Magic 2020 Top 10 Hits Ovnimoon Rec, Vol1','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/psychedelic-trance-magic-2020-top-10-hits-ovnimoon-rec-vol1'),
 (11361,10,'Inpsyde Out - Holographic Universe (ovniep375- Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/inpsyde-out-holographic-universe-ovniep375-ovnimoon-records'),
 (11362,10,'Morrisound - Area 51 (ovniep374- Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/morrisound-area-51-ovniep374-ovnimoon-records'),
 (11363,10,'N3V1773 - Genzen (ovniep373- Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/n3v1773-genzen-ovniep373-ovnimoon-records'),
 (11364,10,'Morego - Astrophile (ovniep374- Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/morego-astrophile-ovniep374-ovnimoon-records'),
 (11365,10,'Sirius Music - Dimensional Doorway (ovniep378- Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sirius-music-dimensional-doorway-ovniep378-ovnimoon-records'),
 (11366,10,'Spectro Senses, Jaia2Gaia - Acid Method (ovniep376- Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/spectro-senses-jaia2gaia-acid-method-ovniep376-ovnimoon-records'),
 (11367,10,'Omnipresent Technology - Peace Love Order Truth (ovniep371- Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/omnipresent-technology-peace-love-order-truth-ovniep371-ovnimoon-records'),
 (11368,10,'Zenix - The Sensation (ovniep367 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/zenix-the-sensation-ovniep367-ovnimoon-records'),
 (11369,10,'Acoustic Pressure - Motional Resistance (ovniep368 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/acoustic-pressure-motional-resistance-ovniep368-ovnimoon-records'),
 (11370,10,'Souls Map - Lucid Systematic Dreams (ovniep365 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/souls-map-lucid-systematic-dreams-ovniep365-ovnimoon-records'),
 (11371,10,'Pineal Eye - C20H25N3O (ovniep364 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/pineal-eye-c20h25n3o-ovniep364-ovnimoon-records'),
 (11372,10,'Abyss - Amateratsu (ovniep363 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/abyss-amateratsu-ovniep363-ovnimoon-records'),
 (11373,10,'Psilosin - Digital Forest (ovniep362 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/psilosin-digital-forest-ovniep362-ovnimoon-records'),
 (11374,10,'Electit - Scattered Dreams (ovniep361 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/electit-scattered-dreams-ovniep361-ovnimoon-records'),
 (11375,10,'Klasher - The Night Rider LP (ovniLP931 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/klasher-the-night-rider-lp-ovnilp931-ovnimoon-records'),
 (11376,10,'Dhyana - Mudra (ovniep360 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/dhyana-mudra-ovniep360-ovnimoon-records'),
 (11377,10,'Rave[n] - Black Reality (ovniep359 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/rave-n-black-reality-ovniep359-ovnimoon-records'),
 (11378,10,'Klasher - The Night Rider EP Side B (ovniep367 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/klasher-the-night-rider-ep-side-b-ovniep367-ovnimoon-records'),
 (11379,10,'Ananda Shanti - Kundalini Tuning (ovniep358 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ananda-shanti-kundalini-tuning-ovniep358-ovnimoon-records'),
 (11380,10,'Rigel - Creativity Portals (OVNICD129 - Ovnimoon records','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/rigel-creativity-portals-ovnicd129-ovnimoon-records'),
 (11381,10,'Psychobass - Alien Contact (ovniep357 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/psychobass-alien-contact-ovniep357-ovnimoon-records'),
 (11382,10,'Klasher - The Night Rider EP Side A (ovniep366 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/klasher-the-night-rider-ep-side-a-ovniep366-ovnimoon-records'),
 (11383,10,'Jedidiah - Effusion (ovniep356 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/jedidiah-effusion-ovniep356-ovnimoon-records'),
 (11384,10,'Hedustma - Forms 03 (OVNICD128 - Ovnimoon records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/hedustma-forms-03-ovnicd128-ovnimoon-records'),
 (11385,10,'Socio - Black Dreams (ovniep355 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/socio-black-dreams-ovniep355-ovnimoon-records'),
 (11386,10,'Synkdelic - Magnetic Ritual (ovniep354 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/synkdelic-magnetic-ritual-ovniep354-ovnimoon-records'),
 (11387,10,'Humanology - Violet (ovniep353 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/humanology-violet-ovniep353-ovnimoon-records'),
 (11388,10,'Alignments, Denature - The Soul (ovniep352 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/alignments-denature-the-soul-ovniep352-ovnimoon-records'),
 (11389,10,'N3V1773 - Ambassadors Of Peace (ovniep351 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/n3v1773-ambassadors-of-peace-ovniep351-ovnimoon-records'),
 (11390,10,'Acid Syndrome - Angular Movement (ovniep350 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/acid-syndrome-angular-movement-ovniep350-ovnimoon-records'),
 (11391,10,'Raindrop - Choices (ovniep349 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/raindrop-choices-ovniep349-ovnimoon-records'),
 (11392,10,'Lemonchill - Connections (ovniep348 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/lemonchill-connections-ovniep348-ovnimoon-records'),
 (11393,10,'Bent - Unreal Honors (ovniep347 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/bent-unreal-honors-ovniep347-ovnimoon-records'),
 (11394,10,'Infernal Gift - Black Dreams (ovniep346 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/infernal-gift-black-dreams-ovniep346-ovnimoon-records'),
 (11395,10,'Audiosonic - Strolling Trough The Acid (ovniep345 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/audiosonic-strolling-trough-the-acid-ovniep345-ovnimoon-records'),
 (11396,10,'Claudio Arditti - Putin Club (ovniep344 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/claudio-arditti-putin-club-ovniep344-ovnimoon-records'),
 (11397,10,'Mana Shield - Visions (ovniep343 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/mana-shield-visions-ovniep343-ovnimoon-records'),
 (11398,10,'Sixsense - Infinite Universe (ovniep342 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sixsense-infinite-universe-ovniep342-ovnimoon-records'),
 (11399,10,'Sabedoria - Space Travel (ovniep341 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sabedoria-space-travel-ovniep341-ovnimoon-records'),
 (11400,10,'Inmd - Brujo (ovniep340 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/inmd-brujo-ovniep340-ovnimoon-records'),
 (11401,10,'Elepho - Kontakt (ovniep338 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/elepho-kontakt-ovniep338-ovnimoon-records'),
 (11402,10,'Dusha - Genuine  (ovniep335 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/dusha-genuine-ovniep335-ovnimoon-records'),
 (11403,10,'Zsanchos - Annyo  (ovniep336 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/zsanchos-annyo-ovniep336-ovnimoon-records'),
 (11404,10,'Ovnimoon &amp; Mind Evolution - Divino Conhecimento  (ovniep337 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ovnimoon-mind-evolution-divino-conhecimento-ovniep337-ovnimoon-records'),
 (11405,10,'PsyStream - The Transmission  (ovniep334 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/psystream-the-transmission-ovniep334-ovnimoon-records'),
 (11406,10,'Spiritual Molecule - Galactic Boarding  (ovniep333 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/spiritual-molecule-galactic-boarding-ovniep333-ovnimoon-records'),
 (11407,10,'Larbaceo - Despierta (ovniep332 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/larbaceo-despierta-ovniep332-ovnimoon-records'),
 (11408,10,'Komfuzium - Abduction (ovniep331 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/komfuzium-abduction-ovniep331-ovnimoon-records'),
 (11409,10,'Ovnimoon &amp; Dekin - Focus In Trance (ovniep330 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ovnimoon-dekin-focus-in-trance-ovniep330-ovnimoon-records'),
 (11410,10,'Cena Balak - Beep (ovniep329 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/cena-balak-beep-ovniep329-ovnimoon-records'),
 (11411,10,'Cosmic Light, Dekin - Universo (ovniep327 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/cosmic-light-dekin-universo-ovniep327-ovnimoon-records'),
 (11412,10,'Blue Bliss - Fairlight In Places (ovniep326 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/blue-bliss-fairlight-in-places-ovniep326-ovnimoon-records'),
 (11413,10,'Lastfloor - Equation Theory (ovniep325 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/lastfloor-equation-theory-ovniep325-ovnimoon-records'),
 (11414,10,'Elepho - Transition  (ovniep324 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/elepho-transition-ovniep324-ovnimoon-records'),
 (11415,10,'Bent - Theory of Everything (ovniep323 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/bent-theory-of-everything-ovniep323-ovnimoon-records'),
 (11416,10,'Electit - Perception Of Awakening (OVNICD127 - Ovnimoon records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/electit-perception-of-awakening-ovnicd127-ovnimoon-records'),
 (11417,10,'Larbaceo - Kuyen (ovniep322 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/larbaceo-kuyen-ovniep322-ovnimoon-records'),
 (11418,10,'Mysteries Of Psytrance V7 (OVNICD126 - Ovnimoon records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/mysteries-of-psytrance-v7-ovnicd126-ovnimoon-records'),
 (11419,10,'Thenaria - Virus Waterfall (ovniep321 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/thenaria-virus-waterfall-ovniep321-ovnimoon-records'),
 (11420,10,'Stereo Space &amp; BrokenHead- Perfect Balance (ovniep320 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/stereo-space-brokenhead-perfect-balance-ovniep320-ovnimoon-records'),
 (11421,10,'Refocus - Concept Of Time (ovniep319 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/refocus-concept-of-time-ovniep319-ovnimoon-records'),
 (11422,10,'Manjushri - Om Namo Shiva (ovniep318 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/manjushri-om-namo-shiva-ovniep318-ovnimoon-records'),
 (11423,10,'SixSense - Fuzed (ovniep317 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sixsense-fuzed-ovniep317-ovnimoon-records'),
 (11424,10,'PsyBuddy - Ecstatic Motion (ovniep316 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/psybuddy-ecstatic-motion-ovniep316-ovnimoon-records'),
 (11425,10,'PsyStream - Mayh (ovniep315 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/psystream-mayh-ovniep315-ovnimoon-records'),
 (11426,10,'Mind-Service - Colliding Dimensions (ovniep314 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/mind-service-colliding-dimensions-ovniep314-ovnimoon-records'),
 (11427,10,'Silent Freedom - Some Respect (ovniep313 - Ovnimoon Records)
                
                
                Silent Freedom','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/silent-freedom-some-respect-ovniep313-ovnimoon-records'),
 (11428,10,'Trizono - Saurus (ovniep312 - Ovnimoon Records)
                
                
                Trizono','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/trizono-saurus-ovniep312-ovnimoon-records'),
 (11429,10,'Spectro Senses &amp; Magnifix - Pandora&#39;s Box (ovniep311 - Ovnimoon Records)
                
                
                Spectro Senses','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/spectro-senses-magnifix-pandoras-box-ovniep311-ovnimoon-records'),
 (11430,10,'Sirian &amp; Ovnimoon - We&#39;re Not Alone (ovniep310 - Ovnimoon Records)
                
                
                Sirian &amp; Ovnimoon','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sirian-ovnimoon-were-not-alone-ovniep310-ovnimoon-records'),
 (11431,10,'Avaha - Sufi (ovniep309 - Ovnimoon Records)
                
                
                Avaha','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/avaha-sufi-ovniep309-ovnimoon-records'),
 (11432,10,'Sixsense &amp; Psymon - Payonta (ovniep308 - Ovnimoon Records)
                
                
                Sixsense &amp; Psymon','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sixsense-psymon-payonta-ovniep308-ovnimoon-records'),
 (11433,10,'Marshmalien - Reinvent Myself (ovniep307 - Ovnimoon Records)
                
                
                Marshmalien','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/marshmalien-reinvent-myself-ovniep307-ovnimoon-records'),
 (11434,10,'Ganapati Trunkadelic - Spiritual Connection (ovniep306 - Ovnimoon Records)
                
                
                Ganapati Trunkadelic','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ganapati-trunkadelic-spiritual-connection-ovniep306-ovnimoon-records'),
 (11435,10,'Jedidiah - Dualidad Del Ser LP (ovniLP930 - Ovnimoon Records)
                
                
                Jedidiah','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/jedidiah-dualidad-del-ser-lp-ovnilp930-ovnimoon-records'),
 (11436,10,'Deep Sky - Beyond Reality (ovniep305 - Ovnimoon Records)
                
                
                Deep Sky','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/deep-sky-beyond-reality-ovniep305-ovnimoon-records'),
 (11437,10,'Jedidiah &amp; Existence - Magna Presence (ovniep304 - Ovnimoon Records)
                
                
                Jedidiah &amp; Existence','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/jedidiah-existence-magna-presence-ovniep304-ovnimoon-records'),
 (11438,10,'Protonix - Cosmic Mantra (ovniep303- Ovnimoon Records)
                
                
                Protonix','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/protonix-cosmic-mantra-ovniep303-ovnimoon-records'),
 (11439,10,'Ovnimoon, Via Axis, ItomLab - Galactic Mantra (Disorder Remix) (ovniep302 - Ovnimoon Records)
                
                
                Ovnimoon, Via Axis, ItomLab','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ovnimoon-via-axis-itomlab-galactic-mantra-disorder-remix-ovniep302-ovnimoon-records'),
 (11440,10,'Rythmo - Age of Angels (ovniep301 - Ovnimoon Records)
                
                
                Rythmo','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/rythmo-age-of-angels-ovniep301-ovnimoon-records'),
 (11441,10,'Claudio Arditi - Portal (ovniep300 - Ovnimoon Records)
                
                
                Claudio Arditi','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/claudio-arditi-portal-ovniep300-ovnimoon-records'),
 (11442,10,'Brainless - India (ovniep299 - Ovnimoon Records)
                
                
                Brainless','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/brainless-india-ovniep299-ovnimoon-records'),
 (11443,10,'Cathar - Lost(ovniep298 - Ovnimoon Records)
                
                
                Cathar','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/cathar-lost-ovniep298-ovnimoon-records'),
 (11444,10,'Psybuddy - Sonic Entities(ovniep297 - Ovnimoon Records)
                
                
                Psybuddy','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/psybuddy-sonic-entities-ovniep297-ovnimoon-records'),
 (11445,10,'Ion Vader - Uplifting Force (ovniep296 - Ovnimoon Records)
                
                
                Ion Vader','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ion-vader-uplifting-force-ovniep296-ovnimoon-records'),
 (11446,10,'Raaisel - Birth (ovniep295 - Ovnimoon Records)
                
                
                Raaisel','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/raaisel-birth-ovniep295-ovnimoon-records'),
 (11447,10,'Heart Of Goa Vol 6 by Ovnimoon (OVNICD124 - Ovnimoon records)
                
                
                V/A','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/heart-of-goa-vol-6-by-ovnimoon-ovnicd124-ovnimoon-records'),
 (11448,10,'Ovnimoon - Different Lifeforms (OVNICD125 - Ovnimoon Records)
                
                
                Ovnimoon','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ovnimoon-different-lifeforms-ovnicd125-ovnimoon-records'),
 (11449,10,'Hanumann &amp; Cryptophonix - Newen Weichafe (ovniep294 - Ovnimoon Records)
                
                
                Hanumann','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/hanumann-cryptophonix-newen-weichafe-ovniep294-ovnimoon-records'),
 (11450,10,'Psymon &amp; Sixsense - Lost Place (ovniep293 - Ovnimoon Records)
                
                
                Sixsense','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/psymon-sixsense-lost-place-ovniep293-ovnimoon-records'),
 (11451,10,'Wizack Twizack - Fly With Us (OVNICD123 - Ovnimoon records)
                
                
                Wizack Twizack','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/wizack-twizack-fly-with-us-ovnicd123-ovnimoon-records'),
 (11452,10,'Elepho - Reconnaissance (ovniLP929 - Ovnimoon Records)
                
                
                Elepho','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/elepho-reconnaissance-ovnilp929-ovnimoon-records'),
 (11453,10,'Unusual Cosmic Process - Hidden Emotions (ovniLP928 - Ovnimoon Records)
                
                
                Unusual Cosmic Process','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/unusual-cosmic-process-hidden-emotions-ovnilp928-ovnimoon-records'),
 (11454,10,'Ovnimoon &amp; Esterim Patchuli - Gypsy Histories (ovniep292 - Ovnimoon Records)
                
                
                Ovnimoon','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ovnimoon-esterim-patchuli-gypsy-histories-ovniep292-ovnimoon-records'),
 (11455,10,'Akoustik - Mantra (ovniep291 - Ovnimoon Records)
                
                
                Akoustik','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/akoustik-mantra-ovniep291-ovnimoon-records'),
 (11456,10,'Dekin - New Horizon (ovniep290 - Ovnimoon Records)
                
                
                Dekin','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/dekin-new-horizon-ovniep290-ovnimoon-records'),
 (11457,10,'Black Sun - Guide to the Galaxy (ovniep289 - Ovnimoon Records)
                
                
                Black Sun','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/black-sun-guide-to-the-galaxy-ovniep289-ovnimoon-records'),
 (11458,10,'Akoustik - Alien Abduction (ovniep288 - Ovnimoon Records)
                
                
                Akoustik','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/akoustik-alien-abduction-ovniep288-ovnimoon-records'),
 (11459,10,'Psychobass - Depths Of Mind (ovniep287 - Ovnimoon Records)
                
                
                Psychobass','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/psychobass-depths-of-mind-ovniep287-ovnimoon-records'),
 (11460,10,'Sixsense - U.F.O. Phenomenon (OVNICD122 - Ovnimoon records)
                
                
                Sixsense','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sixsense-u-f-o-phenomenon-ovnicd122-ovnimoon-records'),
 (11461,10,'Mina &amp; Norma Project - Feel  (ovniep286 - Ovnimoon Records)
                
                
                Mina','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/mina-norma-project-feel-ovniep286-ovnimoon-records'),
 (11462,10,'Sixsense &amp; Psymon - Up to Space (ovniep285 - Ovnimoon Records)
                
                
                Sixsense, Psymon','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sixsense-psymon-up-to-space-ovniep285-ovnimoon-records'),
 (11463,10,'Material Music - Magic Science Of DNA (ovniep284 - Ovnimoon Records)
                
                
                Material Music','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/material-music-magic-science-of-dna-ovniep284-ovnimoon-records'),
 (11464,10,'Sirius Music - Gaias Tears Remix EP (ovniep283 - Ovnimoon Records)
                
                
                Sirius Music','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sirius-music-gaias-tears-remix-ep-ovniep283-ovnimoon-records'),
 (11465,10,'Sirius Music - Gaias Tears LP (ovniLP927 - Ovnimoon Records)
                
                
                Sirius Music','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sirius-music-gaias-tears-lp-ovnilp927-ovnimoon-records'),
 (11466,10,'Vuchur - Microscopic Black Holes (ovniep282 - Ovnimoon Records)
                
                
                Vuchur','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/vuchur-microscopic-black-holes-ovniep282-ovnimoon-records'),
 (11467,10,'Goa Trance Aural Expansion V2 (OVNICD121 - Ovnimoon records)
                
                
                V/A','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/goa-trance-aural-expansion-v2-ovnicd121-ovnimoon-records'),
 (11468,10,'Morrisound  - Intergalactic (ovniep281 - Ovnimoon Records)
                
                
                Morrisound','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/morrisound-intergalactic-ovniep281-ovnimoon-records'),
 (11469,10,'El Zisco - Sometimes (ovniep280 - Ovnimoon Records)
                
                
                El Zisco','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/el-zisco-sometimes-ovniep280-ovnimoon-records'),
 (11470,10,'Mina - Black or White (ovniep279 - Ovnimoon Records)
                
                
                Mina','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/mina-black-or-white-ovniep279-ovnimoon-records'),
 (11471,10,'Holistic - Portal Astral (ovniep278 - Ovnimoon Records)
                
                
                Holistic','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/holistic-portal-astral-ovniep278-ovnimoon-records'),
 (11472,10,'Tlamanik - Out of Paradigma (ovniep277 - Ovnimoon Records)
                
                
                Tlamanik','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/tlamanik-out-of-paradigma-ovniep277-ovnimoon-records'),
 (11473,10,'SixSense - Constant Magic (ovniep276 - Ovnimoon Records)
                
                
                Sixsense','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sixsense-constant-magic-ovniep276-ovnimoon-records'),
 (11474,10,'Raaisel - Mysticism (ovniep275 - Ovnimoon Records)
                
                
                Raaisel','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/raaisel-mysticism-ovniep275-ovnimoon-records'),
 (11475,10,'Psymon &amp; Sixsense - Shiva&#39;Nam(ovniep274 - Ovnimoon Records)
                
                
                Psymon &amp; Sixsense','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/psymon-sixsense-shivanam-ovniep274-ovnimoon-records'),
 (11476,10,'Memorio - Cognizant Structures(ovniep273 - Ovnimoon Records)
                
                
                Memorio','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/memorio-cognizant-structures-ovniep273-ovnimoon-records'),
 (11477,10,'Tottem - The Universe​ (​ovniep272- Ovnimoon Records)
                
                
                Tottem','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/tottem-the-universe-ovniep272-ovnimoon-records'),
 (11478,10,'Mina - Anticipation​(​ovniep271- Ovnimoon Records)
                
                
                Mina','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/mina-anticipation-ovniep271-ovnimoon-records'),
 (11479,10,'Fiery Dawn - In Your Mind (ovniep270 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/fiery-dawn-in-your-mind-ovniep270-ovnimoon-records'),
 (11480,10,'Omel - Knowing (ovniep269 - Ovnimoon Records)
                
                
                Omel','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/omel-knowing-ovniep269-ovnimoon-records'),
 (11481,10,'Second Side - Total Reset (ovniep268 - Ovnimoon Records)
                
                
                Second Side','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/second-side-total-reset-ovniep268-ovnimoon-records'),
 (11482,10,'Psymon, Sixsense - Brain Activity (ovniep267- Ovnimoon Records)
                
                
                Psymon, Sixsense','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/psymon-sixsense-brain-activity-ovniep267-ovnimoon-records'),
 (11483,10,'Thenaria - Outside (ovniep266 - Ovnimoon Records)
                
                
                Thenaria','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/thenaria-outside-ovniep266-ovnimoon-records'),
 (11484,10,'Ananda Shanti - Krishna is Black​ (​ovniep265 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ananda-shanti-krishna-is-black-ovniep265-ovnimoon-records'),
 (11485,10,'MoRsei - Evolution (​ovniep264 - Ovnimoon Records)
                
                
                MoRsei','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/morsei-evolution-ovniep264-ovnimoon-records'),
 (11486,10,'Dusha - II (​ovnilp926 - Ovnimoon Records)
                
                
                Dusha','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/dusha-ii-ovnilp926-ovnimoon-records'),
 (11487,10,'Prematron - Fractals (ovniep263 - Ovnimoon Records)
                
                
                Prematron','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/prematron-fractals-ovniep263-ovnimoon-records'),
 (11488,10,'Psybuddy - Celestial Divers (ovniep262 - Ovnimoon Records)
                
                
                Psybuddy','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/psybuddy-celestial-divers-ovniep262-ovnimoon-records'),
 (11489,10,'Maan - Orbiter (ovniep261 - Ovnimoon Records)
                
                
                Maan','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/maan-orbiter-ovniep261-ovnimoon-records'),
 (11490,10,'Kymatica - Mystic Union (ovniLP924 - Ovnimoon Records)
                
                
                Kymatica','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/kymatica-mystic-union-ovnilp924-ovnimoon-records'),
 (11491,10,'Elegy - Time (ovniep260 - Ovnimoon Records)
                
                
                Elegy','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/elegy-time-ovniep260-ovnimoon-records'),
 (11492,10,'Gaia Temple by Jedidiah (ovniLP925 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/gaia-temple-by-jedidiah-ovnilp925-ovnimoon-records'),
 (11493,10,'Stereologic - Radioactivity (ovniep259 - Ovnimoon Records)
                
                
                Stereologic','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/stereologic-radioactivity-ovniep259-ovnimoon-records-2'),
 (11494,10,'Elastic - Electric Signals (ovniep258 - Ovnimoon Records)
                
                
                Elastic','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/elastic-electric-signals-ovniep258-ovnimoon-records'),
 (11495,10,'Hypnoxock - In Flux (ovniep257 - Ovnimoon Records)
                
                
                Hypnoxock','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/hypnoxock-in-flux-ovniep257-ovnimoon-records'),
 (11496,10,'Pointelin - Elastic Air (ovniep256 - Ovnimoon Records)
                
                
                Pointelin','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/pointelin-elastic-air-ovniep256-ovnimoon-records'),
 (11497,10,'Individuum - Overcoming (ovniLP923 - Ovnimoon Records)
                
                
                Individuum','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/individuum-overcoming-ovnilp923-ovnimoon-records'),
 (11498,10,'Oudviber - Urban Oasis (ovniep255 - Ovnimoon Records)
                
                
                Oudviber','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/oudviber-urban-oasis-ovniep255-ovnimoon-records'),
 (11499,10,'Vuchur - Vibrations (ovniep254 - Ovnimoon Records)
                
                
                Vuchur','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/vuchur-vibrations-ovniep254-ovnimoon-records'),
 (11500,10,'Tiberion - Portal Transform (ovniep253 - Ovnimoon Records)
                
                
                Tiberion','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/tiberion-portal-transform-ovniep253-ovnimoon-records'),
 (11501,10,'Psylenth: Compiled by Ovnimoon (ovnicd119 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/psylenth-compiled-by-ovnimoon-ovnicd119-ovnimoon-records'),
 (11502,10,'Floating Planet - Peak Experience (ovniep252 - Ovnimoon Records)
                
                
                Floating Planet','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/floating-planet-peak-experience-ovniep252-ovnimoon-records'),
 (11503,10,'Summer Sun 2 (ovniep251 - Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/summer-sun-2-ovniep251-ovnimoon-records'),
 (11504,10,'2Spirals - Kundalini (ovniLP922 - Ovnimoon Records)
                
                
                2Spirals','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/2spirals-kundalini-ovnilp922-ovnimoon-records'),
 (11505,10,'El Zisco - Arrival (ovniLP921 - Ovnimoon Records)
                
                
                El Zisco','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/el-zisco-arrival-ovnilp921-ovnimoon-records'),
 (11506,10,'Psymon - Transformation (ovniep250 - Ovnimoon Records)
                
                
                Psymon','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/psymon-transformation-ovniep250-ovnimoon-records'),
 (11507,10,'Evlov - Modern Ritual (ovniLP920 - Ovnimoon Records)
                
                
                Evlov','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/evlov-modern-ritual-ovnilp920-ovnimoon-records'),
 (11508,10,'Ovnimoon - Voyage [ovnicd118] - (Ovnimoon Records)
                
                
                Ovnimoon','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ovnimoon-voyage-ovnicd118-ovnimoon-records'),
 (11509,10,'The Bitzpan - Set Off (ovniep249 - Ovnimoon Records)
                
                
                The Bitzpan','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/the-bitzpan-set-off-ovniep249-ovnimoon-records'),
 (11510,10,'Kri Samadhi, Nervasystem - Merry Pranksters (ovniep248 - Ovnimoon Records)
                
                
                Kri Samadhi, Nervasystem','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/kri-samadhi-nervasystem-merry-pranksters-ovniep248-ovnimoon-records'),
 (11511,10,'Sonic Effects - The Spirit Molecule (ovniep247 - Ovnimoon Records)
                
                
                Sonic Effects','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sonic-effects-the-spirit-molecule-ovniep247-ovnimoon-records'),
 (11512,10,'Mysteries of Psytrance vol. 6 by Ovnimoon [ovnicd117]','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/mysteries-of-psytrance-vol-6-by-ovnimoon-ovnicd117'),
 (11513,10,'Chertex - Except Yourself (ovniep246 - Ovnimoon Records)
                
                
                Chertex','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/chertex-except-yourself-ovniep246-ovnimoon-records'),
 (11514,10,'Alphaxone - Perception (ovniep245 - Ovnimoon Records)
                
                
                Alphaxone','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/alphaxone-perception-ovniep245-ovnimoon-records'),
 (11515,10,'Mentalist - Extraterrestrial Races (ovniep244 - Ovnimoon Records)
                
                
                Mentalist','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/mentalist-extraterrestrial-races-ovniep244-ovnimoon-records'),
 (11516,10,'Isralienn - Psycho Plastik Form (ovnicd116 - Ovnimoon Rec.)
                
                
                Isralienn','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/isralienn-psycho-plastik-form-ovnicd116-ovnimoon-rec'),
 (11517,10,'Norma Project - Creative Minds (ovnicd115 - Ovnimoon Rec.)
                
                
                Norma Project','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/norma-project-creative-minds-ovnicd115-ovnimoon-rec'),
 (11518,10,'Ion Vader - Rising Sun (ovniep243 - Ovnimoon Records)
                
                
                Ion Vader','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ion-vader-rising-sun-ovniep243-ovnimoon-records'),
 (11519,10,'Vuchur - Energy From the Cosmos	(ovniep242 - Ovnimoon Records)
                
                
                Vuchur','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/vuchur-energy-from-the-cosmos-ovniep242-ovnimoon-records'),
 (11520,10,'Ion Vader - Plantas Medicinales (ovniep241 - Ovnimoon Records)
                
                
                Ion Vader','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ion-vader-plantas-medicinales-ovniep241-ovnimoon-records'),
 (11521,10,'Bajo la Planta (ovniep240 - Ovnimoon Records)
                
                
                I.M.D','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/bajo-la-planta-ovniep240-ovnimoon-records'),
 (11522,10,'Ion Vader - Ongaku (ovniep239 - Ovnimoon Records)
                
                
                Ion Vader','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ion-vader-ongaku-ovniep239-ovnimoon-records'),
 (11523,10,'Dusha - Silver River (ovniep238 - Ovnimoon Records)
                
                
                Dusha','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/dusha-silver-river-ovniep238-ovnimoon-records'),
 (11524,10,'Abstrackt Dimension - Seeking The Source (ovniep237 - Ovnimoon Records)
                
                
                Abstrackt Dimension','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/abstrackt-dimension-seeking-the-source-ovniep237-ovnimoon-records'),
 (11525,10,'The Bitzpan - More Than Cotton (ovniep236 - Ovnimoon Records)
                
                
                The Bitzpan','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/the-bitzpan-more-than-cotton-ovniep236-ovnimoon-records'),
 (11526,10,'Lost Shaman - Twilight Turtle (ovniep235 - Ovnimoon Records)
                
                
                Lost Shaman','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/lost-shaman-twilight-turtle-ovniep235-ovnimoon-records-2'),
 (11527,10,'Jedidiah - Terra Sacra (ovniLP919 - Ovnimoon Records)
                
                
                Jedidiah','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/jedidiah-terra-sacra-ovnilp919-ovnimoon-records'),
 (11528,10,'Dimensional Mind Transmission - Oraculum (ovniep234 - Ovnimoon Records)
                
                
                Dimensional Mind Transmission','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/dimensional-mind-transmission-oraculum-ovniep234-ovnimoon-records'),
 (11529,10,'Offlabel - Starseed LP (ovniLP918 - Ovnimoon Records)
                
                
                Offlabel','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/offlabel-starseed-lp-ovnilp918-ovnimoon-records'),
 (11530,10,'Moaiact, Zoologic - Reversing System (ovniep233 - Ovnimoon Records)
                
                
                Moaiact, Zoologic','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/moaiact-zoologic-reversing-system-ovniep233-ovnimoon-records'),
 (11531,10,'One Arc Degree, Trinaural In Dub - Coalescence (ovniep232 - Ovnimoon Records)
                
                
                One Arc Degree, Trinaural In Dub','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/one-arc-degree-trinaural-in-dub-coalescence-ovniep232-ovnimoon-records'),
 (11532,10,'Pink Llama - I Rise (ovniep231 - Ovnimoon Records)
                
                
                Pink Llama','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/pink-llama-i-rise-ovniep231-ovnimoon-records'),
 (11533,10,'Ovnimoon - The Digital Blonde Remixes (ovniep230 - Ovnimoon Records)
                
                
                Ovnimoon','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ovnimoon-the-digital-blonde-remixes-ovniep230-ovnimoon-records'),
 (11534,10,'Kri Samadhi - Unison (ovniep229 - Ovnimoon Records)
                
                
                Kri Samadhi','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/kri-samadhi-unison-ovniep229-ovnimoon-records'),
 (11535,10,'Vlainich - Connecting (ovniep228 - Ovnimoon Records)
                
                
                Vlainich','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/vlainich-connecting-ovniep228-ovnimoon-records'),
 (11536,10,'Akron - Hologram (ovniep227 - Ovnimoon Records)
                
                
                Akron','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/akron-hologram-ovniep227-ovnimoon-records'),
 (11537,10,'Beyond The Singularity (ovnicd114 - Ovnimoon Rec.)
                
                
                Rigel','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/beyond-the-singularity-ovnicd114-ovnimoon-rec'),
 (11538,10,'Beatspy - The Eye of the Desert (ovniep226 - Ovnimoon Records)
                
                
                Beatspy','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/beatspy-the-eye-of-the-desert-ovniep226-ovnimoon-records'),
 (11539,10,'Schy - The Compounds for Better Result (ovniep225 - Ovnimoon Records)
                
                
                Schy','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/schy-the-compounds-for-better-result-ovniep225-ovnimoon-records'),
 (11540,10,'Noiland - Body Play (ovniLP917 - Ovnimoon Records)
                
                
                Noiland','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/noiland-body-play-ovnilp917-ovnimoon-records'),
 (11541,10,'Noiland - The Only Way (ovniep224 - Ovnimoon Records)
                
                
                Noiland','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/noiland-the-only-way-ovniep224-ovnimoon-records'),
 (11542,10,'Raindrop - From the Void (ovniep223 - Ovnimoon Records)
                
                
                Raindrop','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/raindrop-from-the-void-ovniep223-ovnimoon-records'),
 (11543,10,'Rishi - Shift Happens!! (ovniep222 - Ovnimoon Records)
                
                
                Rishi','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/rishi-shift-happens-ovniep222-ovnimoon-records'),
 (11544,10,'Trycerapt - Jade Eyes (ovniep221 - Ovnimoon Records)
                
                
                Trycerapt','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/trycerapt-jade-eyes-ovniep221-ovnimoon-records'),
 (11545,10,'Microlin - Just for You (ovniep220 - Ovnimoon Records)
                
                
                Microlin','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/microlin-just-for-you-ovniep220-ovnimoon-records'),
 (11546,10,'Spectro Senses, Synthetic Vision - Bansuri (ovniep219 - Ovnimoon Records)
                
                
                Spectro Senses, Synthetic Vision','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/spectro-senses-synthetic-vision-bansuri-ovniep219-ovnimoon-records'),
 (11547,10,'JaguarTree - Music of the Spheres (ovniep218 - Ovnimoon Records)
                
                
                JaguarTree','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/jaguartree-music-of-the-spheres-ovniep218-ovnimoon-records'),
 (11548,10,'Spectro Senses - The Frequency (ovniep217 - Ovnimoon Records)
                
                
                Spectro Senses','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/spectro-senses-the-frequency-ovniep217-ovnimoon-records'),
 (11549,10,'Inducer - Riot (ovniep216 - Ovnimoon Records)
                
                
                Inducer','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/inducer-riot-ovniep216-ovnimoon-records'),
 (11550,10,'Offlabel - Mystic Force (ovniep215 - Ovnimoon Records)
                
                
                Offlabel','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/offlabel-mystic-force-ovniep215-ovnimoon-records'),
 (11551,10,'Morden Laiv - Solstice (ovniep214 - Ovnimoon Records)
                
                
                Morden Laiv','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/morden-laiv-solstice-ovniep214-ovnimoon-records'),
 (11552,10,'Quantus - Kinetic Energy [ovniep213] (Ovnimoon Records)
                
                
                Quantus','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/quantus-kinetic-energy-ovniep213-ovnimoon-records'),
 (11553,10,'Tripy - Lucid Dream [ovniep212] (Ovnimoon Records)
                
                
                Tripy','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/tripy-lucid-dream-ovniep212-ovnimoon-records'),
 (11554,10,'Offlabel - Lost Ritual [ovniep211] (Ovnimoon Records)
                
                
                Offlabel','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/offlabel-lost-ritual-ovniep211-ovnimoon-records'),
 (11555,10,'Perceptors - Real Virtuality [ovniep210] (Ovnimoon Records)
                
                
                Perceptors','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/perceptors-real-virtuality-ovniep210-ovnimoon-records'),
 (11556,10,'The Key - Back to the Roots [ovniep209] (Ovnimoon Records)
                
                
                The Key','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/the-key-back-to-the-roots-ovniep209-ovnimoon-records'),
 (11557,10,'Offlabel - Starseed [ovniep208] (Ovnimoon Records)
                
                
                Offlabel','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/offlabel-starseed-ovniep208-ovnimoon-records'),
 (11558,10,'Tlamanik - Beyond Physical Cosmology [ovniep207] (Ovnimoon Records)
                
                
                Tlamanik','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/tlamanik-beyond-physical-cosmology-ovniep207-ovnimoon-records'),
 (11559,10,'Yar Zaa &amp; Elegy - Whats True Strangers	[ovniep206] (Ovnimoon Records)
                
                
                Yar Zaa &amp; Elegy','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/yar-zaa-elegy-whats-true-strangers-ovniep206-ovnimoon-records'),
 (11560,10,'Kri Samadhi - Konx om Pax (ovnicd112 - Ovnimoon Rec.)
                
                
                Kri Samadhi','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/kri-samadhi-konx-om-pax-ovnicd112-ovnimoon-rec'),
 (11561,10,'Jikukan vol. 3 - The Space-Time Barrier by Rigel 2CD (ovnicd111 - Ovnimoon Rec.)
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/jikukan-vol-3-the-space-time-barrier-by-rigel-2cd-ovnicd111-ovnimoon-rec'),
 (11562,10,'Terapeutica - Psychotherapy [ovnicd110]
                
                
                Terapeutica','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/terapeutica-psychotherapy-ovnicd110'),
 (11563,10,'Jedidiah - Shamanic Experience (ovniLP916 - Ovnimoon Records)
                
                
                Jedidiah','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/jedidiah-shamanic-experience-ovnilp916-ovnimoon-records'),
 (11564,10,'Milk &amp; Honey VA by Rhino and Ovnimoon [ovnilp915] (Ovnimoon Records)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/milk-honey-va-by-rhino-and-ovnimoon-ovnilp915-ovnimoon-records'),
 (11565,10,'Unusual Cosmic Process - Aerial [ovnilp914] (Ovnimoon Records)
                
                
                Unusual Cosmic Process','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/unusual-cosmic-process-aerial-ovnilp914-ovnimoon-records'),
 (11566,10,'Sheewton - Distant Thoughts [ovniLP913] (Ovnimoon Records)
                
                
                Sheewton','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sheewton-distant-thoughts-ovnilp913-ovnimoon-records'),
 (11567,10,'Lunatica - Double Trip [ovniep205] (Ovnimoon Records)
                
                
                Lunatica','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/lunatica-double-trip-ovniep205-ovnimoon-records'),
 (11568,10,'Natural Life Essence - Forms of Life EP [ovniep203] (Ovnimoon Records)
                
                
                Natural Life Essence','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/natural-life-essence-forms-of-life-ep-ovniep203-ovnimoon-records'),
 (11569,10,'Jedidiah - Equilibrium EP [ovniep204] (Ovnimoon Records)
                
                
                Jedidiah','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/jedidiah-equilibrium-ep-ovniep204-ovnimoon-records'),
 (11570,10,'System E - Warrior Spirit (ovniep202)
                
                
                System E','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/system-e-warrior-spirit-ovniep202'),
 (11571,10,'Jedidiah - Aelohim Chill
                
                
                Jedidiah','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/jedidiah-aelohim-chill'),
 (11572,10,'Declaration of Unity - Intercerebral
                
                
                Declaration of Unity','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/declaration-of-unity-intercerebral'),
 (11573,10,'Sine Eye - Electronic Setup
                
                
                Sine Eye','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sine-eye-electronic-setup'),
 (11574,10,'Phoma - Dream Creator
                
                
                Phoma','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/phoma-dream-creator'),
 (11575,10,'Dimmat - Three Kingdoms
                
                
                Dimmat','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/dimmat-three-kingdoms'),
 (11576,10,'Nature - Embodying The Powers of Nature
                
                
                Nature','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/nature-embodying-the-powers-of-nature'),
 (11577,10,'SwingTek - Hybrid Vibration
                
                
                SwingTek','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/swingtek-hybrid-vibration'),
 (11578,10,'Spectro Senses - Sacred Tree
                
                
                Spectro Senses','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/spectro-senses-sacred-tree'),
 (11579,10,'Mechanix - Mechanical Moon
                
                
                Mechanix','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/mechanix-mechanical-moon'),
 (11580,10,'Raindrop - Rainmaker
                
                
                Raindrop','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/raindrop-rainmaker'),
 (11581,10,'System E - Evolution of Mankind
                
                
                System E','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/system-e-evolution-of-mankind'),
 (11582,10,'Trinaural in Dub - Haiku
                
                
                Trinaural in Dub','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/trinaural-in-dub-haiku'),
 (11583,10,'Aslan One - Saturday Night Fever
                
                
                Aslan One','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/aslan-one-saturday-night-fever'),
 (11584,10,'Proxeeus - The Stars are Right
                
                
                Proxeeus','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/proxeeus-the-stars-are-right'),
 (11585,10,'Mozinet - Retro
                
                
                Mozinet','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/mozinet-retro'),
 (11586,10,'Terapeutica - Spring Flowers
                
                
                Terapeutica','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/terapeutica-spring-flowers'),
 (11587,10,'The Paco Project - Mind Trap
                
                
                The Paco Project','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/the-paco-project-mind-trap'),
 (11588,10,'Amos - Illusions of Tomorrow
                
                
                Amos','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/amos-illusions-of-tomorrow'),
 (11589,10,'Psypheric &amp; Optical Report - Astral
                
                
                Psypheric &amp; Optical Report','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/psypheric-optical-report-astral'),
 (11590,10,'Elegy - Cosmic Sonification
                
                
                Elegy','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/elegy-cosmic-sonification'),
 (11591,10,'Psytellite - Between Earth &amp; Sky
                
                
                Psytellite','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/psytellite-between-earth-sky'),
 (11592,10,'Kundalini - And So On
                
                
                Kundalini','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/kundalini-and-so-on'),
 (11593,10,'Anyma - Awakened Mind
                
                
                Anyma','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/anyma-awakened-mind'),
 (11594,10,'Mysteries Of Psytrance v.5 by Ovnimoon
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/mysteries-of-psytrance-v-5-by-ovnimoon'),
 (11595,10,'Two Faces - I Want Something New
                
                
                Two Faces','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/two-faces-i-want-something-new'),
 (11596,10,'Vimana - Psychonautical Science
                
                
                Vimana','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/vimana-psychonautical-science'),
 (11597,10,'Psypheric - Problem Space [ovniLP909]','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/psypheric-problem-space-ovnilp909'),
 (11598,10,'Dimmat - Karma [ovniLP910]
                
                
                Dimmat','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/dimmat-karma-ovnilp910'),
 (11599,10,'Phantom Sentinel - Observer - [OVNILP908]
                
                
                Phantom Sentinel','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/phantom-sentinel-observer-ovnilp908'),
 (11600,10,'Convergent Evolution - Evolutio - [OVNILP906]
                
                
                Convergent Evolution','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/convergent-evolution-evolutio-ovnilp906'),
 (11601,10,'Flux Natura - A Journey From Babylon - [OVNILP905]
                
                
                Flux Natura','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/flux-natura-a-journey-from-babylon-ovnilp905'),
 (11602,10,'Psyspheric - Virus - [OVNILP903]
                
                
                Psyspheric','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/psyspheric-virus-ovnilp903'),
 (11603,10,'Hinkstep - Sunrise From The Treetops - [OVNILP904]
                
                
                Hinkstep','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/hinkstep-sunrise-from-the-treetops-ovnilp904'),
 (11604,10,'Jikukan vol. 2 - The Summoning By Rigel 2CD
                
                
                Rigel','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/jikukan-vol-2-the-summoning-by-rigel-2cd'),
 (11605,10,'Rishi - Optical Blur
                
                
                Rishi','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/rishi-optical-blur'),
 (11606,10,'Good Vibes V2 (2CD): by Ovnimoon, Pulsar &amp; Djane Gaby
                
                
                Ovnimoon, Pulsar &amp; Djane Gaby','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/good-vibes-v2-2cd-by-ovnimoon-pulsar-djane-gaby'),
 (11607,10,'Psychomotorica - Wind Refuge
                
                
                Psychomotorica','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/psychomotorica-wind-refuge'),
 (11608,10,'Angel Esteban - Religious Experience
                
                
                Angel Esteban','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/angel-esteban-religious-experience'),
 (11609,10,'Optical Report - Time Window
                
                
                Optical Report','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/optical-report-time-window'),
 (11610,10,'Liquid Frame - Just For Today
                
                
                Liquid Frame','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/liquid-frame-just-for-today'),
 (11611,10,'Tech Tone - Musicologia
                
                
                Tech Tone','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/tech-tone-musicologia'),
 (11612,10,'Zweep Vs deSh - Citizens of the Cosmos
                
                
                Zweep Vs deSh','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/zweep-vs-desh-citizens-of-the-cosmos'),
 (11613,10,'Sine Eye - Genesis','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sine-eye-genesis'),
 (11614,10,'Anyma - Blessing in Disguise
                
                
                Anyma','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/anyma-blessing-in-disguise'),
 (11615,10,'Atacama - Halluzination Speak
                
                
                Atacama','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/atacama-halluzination-speak'),
 (11616,10,'Norma Project - Illusion
                
                
                Norma Project','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/norma-project-illusion'),
 (11617,10,'Madness Express - Over and Over
                
                
                Madness Express','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/madness-express-over-and-over'),
 (11618,10,'Theseus - Different Machine
                
                
                Theseus','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/theseus-different-machine'),
 (11619,10,'Merlin&#39;s Apprentice - Nirvana
                
                
                Merlin&#39;s Apprentice','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/merlins-apprentice-nirvana'),
 (11620,10,'Psypheric - Human Zodiac
                
                
                Psypheric','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/psypheric-human-zodiac'),
 (11621,10,'Drop Control - Digital Consciousness
                
                
                Drop Control','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/drop-control-digital-consciousness'),
 (11622,10,'Declaration of Unity - Optical Delusion EP
                
                
                Declaration of Unity','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/declaration-of-unity-optical-delusion-ep'),
 (11623,10,'AslanOne - Starsailor
                
                
                AslanOne','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/aslanone-starsailor'),
 (11624,10,'The Moonbeats V/A Compiled By Maiia - Downtempo / Dub [OVNILP907]
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/the-moonbeats-v-a-compiled-by-maiia-downtempo-dub-ovnilp907'),
 (11625,10,'Elegy - Reflection
                
                
                Elegy','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/elegy-reflection'),
 (11626,10,'Holistic
                
                
                Ovnimoon','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/holistic'),
 (11627,10,'Life EP
                
                
                Nature &amp; Norma Project','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/life-ep'),
 (11628,10,'Hypnotico
                
                
                Ascent &amp; Nature','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/hypnotico'),
 (11629,10,'Daydreaming EP
                
                
                Bioterranean','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/daydreaming-ep'),
 (11630,10,'Don&#39;t Wake Me Up! (Remixes)
                
                
                Phantom Sentinel','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/dont-wake-me-up-remixes'),
 (11631,10,'Tornado EP
                
                
                3G_Kon','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/tornado-ep'),
 (11632,10,'Lock&#39;n Load EP
                
                
                Mental Control','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/lockn-load-ep'),
 (11633,10,'No Strings on Us EP
                
                
                Spectral Touch','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/no-strings-on-us-ep'),
 (11634,10,'Direct Motion EP
                
                
                Limbo','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/direct-motion-ep'),
 (11635,10,'Healing Lights v3 by DJane Gaby
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/healing-lights-v3-by-djane-gaby'),
 (11636,10,'Trancemutation &amp; Superlight in the Darkness Remixes
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/trancemutation-superlight-in-the-darkness-remixes'),
 (11637,10,'Drops
                
                
                Paradigm','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/drops'),
 (11638,10,'Voice From a Distance
                
                
                Norma Project','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/voice-from-a-distance'),
 (11639,10,'Dreamology
                
                
                Party Heroes','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/dreamology'),
 (11640,10,'Heart of Goa v.3 - Compiled by Ovnimoon
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/heart-of-goa-v-3-compiled-by-ovnimoon'),
 (11641,10,'Naturaleza y Cuarzo
                
                
                Alupran','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/naturaleza-y-cuarzo'),
 (11642,10,'Ajna - Search for the Divine (ovnicd094 / Ovnimoon Records)
                
                
                Ajna','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ajna-search-for-the-divine-ovnicd094-ovnimoon-records'),
 (11643,10,'Vintage World EP
                
                
                Shogan','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/vintage-world-ep'),
 (11644,10,'I Want Energy EP
                
                
                ZeoLogic','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/i-want-energy-ep'),
 (11645,10,'Night Fair EP
                
                
                Nature','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/night-fair-ep'),
 (11646,10,'Sky in Diamonds
                
                
                Maiia303','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sky-in-diamonds'),
 (11647,10,'Precious Journey EP
                
                
                DalShar Project','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/precious-journey-ep'),
 (11648,10,'The Second Warp
                
                
                MANMACHINE','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/the-second-warp'),
 (11649,10,'Formula Occurrences EP
                
                
                Yum Kaax','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/formula-occurrences-ep'),
 (11650,10,'Convenience EP
                
                
                Matias Prieto','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/convenience-ep'),
 (11651,10,'Forest Ritual EP
                
                
                Nature','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/forest-ritual-ep'),
 (11652,10,'Nature Creations
                
                
                Ascent','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/nature-creations'),
 (11653,10,'Eye of the Storm
                
                
                Beatfarmer','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/eye-of-the-storm'),
 (11654,10,'Nirvana Network
                
                
                Labyr1nth','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/nirvana-network'),
 (11655,10,'Anthology
                
                
                Spirit Architect','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/anthology'),
 (11656,10,'Heart of Goa Vol. 2
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/heart-of-goa-vol-2'),
 (11657,10,'Mysticism
                
                
                Elgiva','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/mysticism'),
 (11658,10,'Jikukan: The Space-Time Chronicles - Compiled by Rigel
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/jikukan-the-space-time-chronicles-compiled-by-rigel'),
 (11659,10,'Shadows EP
                
                
                Argonnight','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/shadows-ep'),
 (11660,10,'Starseed Transmissions EP
                
                
                Lunar feat. Sunus','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/starseed-transmissions-ep'),
 (11661,10,'Mesopotamia EP
                
                
                Moon Tripper','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/mesopotamia-ep-2'),
 (11662,10,'Play of Light EP
                
                
                Norma Project','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/play-of-light-ep-2'),
 (11663,10,'Nemophilist
                
                
                Ren Toudu','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/nemophilist'),
 (11664,10,'Paz (Peace) Vol. 2 - compiled by Ovnimoon &amp; Itzadragon
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/paz-peace-vol-2-compiled-by-ovnimoon-itzadragon'),
 (11665,10,'Om, Ovnimoon - Viaje al Interior (ovnicd080 / Ovnimoon Records)
                
                
                OM (aka Ovnimoon)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/om-ovnimoon-viaje-al-interior-ovnicd080-ovnimoon-records'),
 (11666,10,'Fruity Juices - compiled by Ovnimoon
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/fruity-juices-compiled-by-ovnimoon'),
 (11667,10,'Colors of Goa - Compiled by Ovnimoon and Nova Fractal
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/colors-of-goa-compiled-by-ovnimoon-and-nova-fractal'),
 (11668,10,'Rock &amp; Roller
                
                
                Liquid Space &amp; Doctor GoA','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/rock-roller'),
 (11669,10,'Inside
                
                
                Elegy','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/inside'),
 (11670,10,'Ways To Dream
                
                
                Norma Project','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ways-to-dream'),
 (11671,10,'Back To Trance
                
                
                3 Access &amp; You','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/back-to-trance'),
 (11672,10,'The Perfect Element
                
                
                Ascent and Argus','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/the-perfect-element'),
 (11673,10,'Patterns in Evolution
                
                
                Psycho Abstract','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/patterns-in-evolution'),
 (11674,10,'Inner Beats
                
                
                DJ Kundalini','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/inner-beats'),
 (11675,10,'Rituals Of The Mind
                
                
                Moon Tripper','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/rituals-of-the-mind'),
 (11676,10,'The Consciousness
                
                
                Technology','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/the-consciousness'),
 (11677,10,'Planet Bong
                
                
                Frosty Fennic','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/planet-bong'),
 (11678,10,'Sun Trip
                
                
                Shogan','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/sun-trip'),
 (11679,10,'Charisma
                
                
                3 Access &amp; You','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/charisma'),
 (11680,10,'Heart of Goa - Compiled by Ovnimoon
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/heart-of-goa-compiled-by-ovnimoon'),
 (11681,10,'Astral Clouds
                
                
                Trinodia','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/astral-clouds'),
 (11682,10,'Ghost Network
                
                
                Barby','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ghost-network'),
 (11683,10,'The Acid Sessions Vol. 3
                
                
                Liquid Rainbow','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/the-acid-sessions-vol-3'),
 (11684,10,'Cannahuasca
                
                
                Chemical Content 1','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/cannahuasca'),
 (11685,10,'Virtual Reality
                
                
                Swing 8 &amp; N.A.S.A.','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/virtual-reality'),
 (11686,10,'Nippy State Of Mind
                
                
                Frosty Fennic','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/nippy-state-of-mind'),
 (11687,10,'Walking On The Moon
                
                
                Norma Project','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/walking-on-the-moon'),
 (11688,10,'Ancient Illusionarium
                
                
                Flucturion 2.0','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ancient-illusionarium'),
 (11689,10,'Droplets From the Matrix
                
                
                Solar Spectrum','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/droplets-from-the-matrix'),
 (11690,10,'PsiloCybian - Dreamtime
                
                
                PsiloCybian','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/psilocybian-dreamtime'),
 (11691,10,'Transitions in Trance Vol. 2 - Compiled by Ovnimoon
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/transitions-in-trance-vol-2-compiled-by-ovnimoon'),
 (11692,10,'Moment
                
                
                Ascent &amp; Argonnight','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/moment'),
 (11693,10,'Portal Pass
                
                
                Samas','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/portal-pass'),
 (11694,10,'Natural Vibes
                
                
                WeiRdel','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/natural-vibes'),
 (11695,10,'Gifts From Elves
                
                
                Elgiva','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/gifts-from-elves'),
 (11696,10,'Retrodelica
                
                
                Necton','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/retrodelica'),
 (11697,10,'Enigma
                
                
                Awareness','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/enigma'),
 (11698,10,'Secret Place
                
                
                Ascent','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/secret-place'),
 (11699,10,'Space Calling
                
                
                Yar Zaa','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/space-calling'),
 (11700,10,'Hinkstep - Out Inner Space
                
                
                Hinkstep','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/hinkstep-out-inner-space'),
 (11701,10,'Beyond Sight
                
                
                Ascent &amp; Argus','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/beyond-sight'),
 (11702,10,'It&#39;s a Story
                
                
                Alchemix','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/its-a-story'),
 (11703,10,'Wild West
                
                
                Avant Garde','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/wild-west'),
 (11704,10,'Mundo Progresivo - Compiled by Lupin
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/mundo-progresivo-compiled-by-lupin'),
 (11705,10,'Source Energy
                
                
                Labyr1nth','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/source-energy'),
 (11706,10,'High Tech Computer
                
                
                Perceptors','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/high-tech-computer'),
 (11707,10,'Forestphonic
                
                
                Astro-D','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/forestphonic'),
 (11708,10,'Trancemutation of the Mind
                
                
                Ovnimoon','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/trancemutation-of-the-mind'),
 (11709,10,'Winter Sun
                
                
                Lost Shaman','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/winter-sun'),
 (11710,10,'Shiftin&#39;
                
                
                SyncTronik','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/shiftin'),
 (11711,10,'Inverse Gravity
                
                
                Predators','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/inverse-gravity'),
 (11712,10,'Spiritual Level
                
                
                Akoustik','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/spiritual-level'),
 (11713,10,'Mysteries of Psytrance Vol. 3 - Compiled by Ovnimoon
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/mysteries-of-psytrance-vol-3-compiled-by-ovnimoon'),
 (11714,10,'The Ark
                
                
                Shake','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/the-ark'),
 (11715,10,'The Call of Goa - Compiled by Nova Fractal
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/the-call-of-goa-compiled-by-nova-fractal'),
 (11716,10,'Lost in Sound
                
                
                Amos','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/lost-in-sound'),
 (11717,10,'Genetic Freak
                
                
                Rollercoaster','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/genetic-freak'),
 (11718,10,'Transformation of Light
                
                
                Hotep','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/transformation-of-light'),
 (11719,10,'Space Machine
                
                
                Modul8','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/space-machine'),
 (11720,10,'Good Vibes - compiled by Pulsar &amp; Ovnimoon
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/good-vibes-compiled-by-pulsar-ovnimoon'),
 (11721,10,'Retrospect 2
                
                
                Necton','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/retrospect-2'),
 (11722,10,'Tripped on Fizz
                
                
                Chitoon','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/tripped-on-fizz'),
 (11723,10,'No Fear
                
                
                SaseK','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/no-fear'),
 (11724,10,'Imaginarium
                
                
                Elegy','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/imaginarium'),
 (11725,10,'Electric Booze
                
                
                SyncTronik','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/electric-booze'),
 (11726,10,'Progressive Pathways - compiled by Ovnimoon &amp; Rigel
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/progressive-pathways-compiled-by-ovnimoon-rigel'),
 (11727,10,'Lost And Found
                
                
                Amos','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/lost-and-found'),
 (11728,10,'In Transit
                
                
                Twin Shape','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/in-transit'),
 (11729,10,'Luv Stimulation
                
                
                Shake','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/luv-stimulation'),
 (11730,10,'Four Oak Trees
                
                
                Ascent and Argus','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/four-oak-trees'),
 (11731,10,'Spirit of the Machine
                
                
                Man Machine','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/spirit-of-the-machine'),
 (11732,10,'Loyalty To The Party
                
                
                Swell','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/loyalty-to-the-party'),
 (11733,10,'Mind Attraction
                
                
                Beatspy','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/mind-attraction'),
 (11734,10,'Archaea
                
                
                Main Sequence Star','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/archaea'),
 (11735,10,'Colored Reality
                
                
                Ascent','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/colored-reality'),
 (11736,10,'The Beginning
                
                
                Avant Garde','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/the-beginning'),
 (11737,10,'Retrospect
                
                
                Necton','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/retrospect'),
 (11738,10,'I Missed A Heart Beat - Remixes
                
                
                Lemonchill','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/i-missed-a-heart-beat-remixes'),
 (11739,10,'Long Day, Over Through The Night
                
                
                Beatfarmer','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/long-day-over-through-the-night'),
 (11740,10,'Infinity Is Now
                
                
                Labyr1nth','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/infinity-is-now'),
 (11741,10,'528Hz Frequency Of Love
                
                
                Terapeutica','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/528hz-frequency-of-love'),
 (11742,10,'Blinded By Science
                
                
                Phoma','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/blinded-by-science'),
 (11743,10,'Seperate Reality
                
                
                Astro-D &amp; Friends','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/seperate-reality'),
 (11744,10,'The Hopi Prophecy
                
                
                Necton','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/the-hopi-prophecy'),
 (11745,10,'Oraculo
                
                
                Lupin','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/oraculo'),
 (11746,10,'Time Pattern Remixes
                
                
                Techyon','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/time-pattern-remixes'),
 (11747,10,'Deep Question
                
                
                Dream Stalker','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/deep-question'),
 (11748,10,'Temporalising Space
                
                
                Psychophysical Transcripts','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/temporalising-space'),
 (11749,10,'The Young Man on Acid Vol. 2 - compiled by Pick
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/the-young-man-on-acid-vol-2-compiled-by-pick'),
 (11750,10,'Mantra
                
                
                Beatfarmer','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/mantra'),
 (11751,10,'One - compiled by Digital Yonkis
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/one-compiled-by-digital-yonkis'),
 (11752,10,'Nostalgic EP
                
                
                DJ Kundalini','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/nostalgic-ep'),
 (11753,10,'Buddhism
                
                
                Lemonchill','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/buddhism'),
 (11754,10,'Storm Riders - compiled by Mind Storm
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/storm-riders-compiled-by-mind-storm'),
 (11755,10,'Little Dendrites
                
                
                Dual Barrell','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/little-dendrites'),
 (11756,10,'Fractal Landscape
                
                
                Nova Fractal','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/fractal-landscape'),
 (11757,10,'On Ground
                
                
                Kota','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/on-ground'),
 (11758,10,'Shakti
                
                
                Maiia','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/shakti'),
 (11759,10,'Alchemix - Brain in Action (OVNICD039 / Ovnimoon Records)
                
                
                Alchemix','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/alchemix-brain-in-action-ovnicd039-ovnimoon-records'),
 (11760,10,'Swarm Of Yellow Flying Monkeys
                
                
                Psilocybian','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/swarm-of-yellow-flying-monkeys'),
 (11761,10,'Anthology 2012
                
                
                Meller','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/anthology-2012'),
 (11762,10,'Shower Of Sparks
                
                
                Tectum','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/shower-of-sparks'),
 (11763,10,'Ooung
                
                
                Chitoon','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ooung'),
 (11764,10,'Holographic Remixes
                
                
                Ovnimoon','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/holographic-remixes'),
 (11765,10,'Aural Expansion - compiled by Ovnimoon
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/aural-expansion-compiled-by-ovnimoon'),
 (11766,10,'Defenders Of The Moon
                
                
                Mental Flow','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/defenders-of-the-moon'),
 (11767,10,'Progressive Textures - compiled by Ovnimoon
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/progressive-textures-compiled-by-ovnimoon'),
 (11768,10,'Neko
                
                
                Mr. Fer','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/neko'),
 (11769,10,'Time Shifter
                
                
                Lost Shaman','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/time-shifter'),
 (11770,10,'Psy-Fi Science
                
                
                Opposite8','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/psy-fi-science'),
 (11771,10,'Spirit Of The Moon
                
                
                Phoma','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/spirit-of-the-moon'),
 (11772,10,'Espacio de Mezclas
                
                
                Lupin','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/espacio-de-mezclas'),
 (11773,10,'Kar Bura
                
                
                Samas','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/kar-bura'),
 (11774,10,'Zen Riot
                
                
                Yar Zaa','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/zen-riot'),
 (11775,10,'Beyond
                
                
                Voxel9','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/beyond'),
 (11776,10,'Chile Psytrance Vol. 2 - compiled by Ovnimoon
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/chile-psytrance-vol-2-compiled-by-ovnimoon'),
 (11777,10,'Spring Back
                
                
                Mental Flow','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/spring-back'),
 (11778,10,'Voices From The Spheres
                
                
                Mother Womb','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/voices-from-the-spheres'),
 (11779,10,'Chaosphere
                
                
                Pranayama','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/chaosphere'),
 (11780,10,'Contact
                
                
                Daniel Lesden','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/contact'),
 (11781,10,'Quasar
                
                
                Barby','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/quasar'),
 (11782,10,'Reshaping Reality
                
                
                Spirit Architect','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/reshaping-reality-2'),
 (11783,10,'Dub In The Roots EP
                
                
                BlueBliss','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/dub-in-the-roots-ep-2'),
 (11784,10,'Reptilian EP
                
                
                Reptilian Renegades','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/reptilian-ep'),
 (11785,10,'Signals From The Surface - The Essential Collection
                
                
                Ovnimoon','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/signals-from-the-surface-the-essential-collection'),
 (11786,10,'The Observers EP
                
                
                Simply Wave','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/the-observers-ep'),
 (11787,10,'Matices EP
                
                
                Xplicit','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/matices-ep'),
 (11788,10,'Psychedelique
                
                
                Smoke Sign','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/psychedelique'),
 (11789,10,'Infiltrator
                
                
                Barby','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/infiltrator'),
 (11790,10,'Intangible
                
                
                Ren Toudu','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/intangible'),
 (11791,10,'Faith
                
                
                Side Winder','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/faith'),
 (11792,10,'Patchouli EP
                
                
                Float','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/patchouli-ep'),
 (11793,10,'Alchemix - The Visions Begin (OVNICD025 / Ovnimoon Records)
                
                
                Alchemix','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/alchemix-the-visions-begin-ovnicd025-ovnimoon-records'),
 (11794,10,'Titan
                
                
                Barby','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/titan'),
 (11795,10,'Summer Sun VA
                
                
                Lupin &amp; Lightsphere','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/summer-sun-va'),
 (11796,10,'Fragmented Mind EP
                
                
                BlueBliss','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/fragmented-mind-ep'),
 (11797,10,'The First Vibration
                
                
                Earthspace','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/the-first-vibration'),
 (11798,10,'Outside World EP
                
                
                Paralogue','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/outside-world-ep'),
 (11799,10,'Stargazing
                
                
                Trinodia','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/stargazing'),
 (11800,10,'Light Source EP
                
                
                Elegy &amp; Suduaya','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/light-source-ep'),
 (11801,10,'Mysteries of Psytrance Vol. 2 - compiled by Ovnimoon
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/mysteries-of-psytrance-vol-2-compiled-by-ovnimoon'),
 (11802,10,'Green Temple EP
                
                
                Ren Toudu','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/green-temple-ep'),
 (11803,10,'Feelings - compiled by Ovnimoon
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/feelings-compiled-by-ovnimoon'),
 (11804,10,'The Young Man on Acid Vol. 1 - compiled by Pick
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/the-young-man-on-acid-vol-1-compiled-by-pick'),
 (11805,10,'Enrredos EP
                
                
                Gesh','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/enrredos-ep'),
 (11806,10,'Altona
                
                
                OM (aka Ovnimoon)','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/altona'),
 (11807,10,'Persepciones EP
                
                
                Rollercoaster','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/persepciones-ep'),
 (11808,10,'Paz (Peace) Vol. 1 - compiled by Ovnimoon
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/paz-peace-vol-1-compiled-by-ovnimoon'),
 (11809,10,'Other Side EP
                
                
                Gesh','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/other-side-ep'),
 (11810,10,'Transitions in Trance Vol. 1 - compiled by Ovnimoon
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/transitions-in-trance-vol-1-compiled-by-ovnimoon'),
 (11811,10,'Times Of Changes EP
                
                
                Expedizionika','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/times-of-changes-ep'),
 (11812,10,'Ovnimoon Records Winter 2011 Figurado
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ovnimoon-records-winter-2011-figurado'),
 (11813,10,'Piscodelia EP
                
                
                Haggen','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/piscodelia-ep'),
 (11814,10,'Ovnimoon Records Fall 2011 Figurado
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ovnimoon-records-fall-2011-figurado'),
 (11815,10,'Ray Of Light EP
                
                
                Astropilot','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ray-of-light-ep'),
 (11816,10,'Ovnimoon Records Summer 2011 Figurado
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ovnimoon-records-summer-2011-figurado'),
 (11817,10,'No Veo TV EP
                
                
                Gao','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/no-veo-tv-ep'),
 (11818,10,'Ovnimoon Records Spring 2011 Figurado
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/ovnimoon-records-spring-2011-figurado'),
 (11819,10,'Nalca EP
                
                
                Electryxeed','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/nalca-ep'),
 (11820,10,'Z.A. EP
                
                
                Amanito','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/z-a-ep'),
 (11821,10,'Paratechnology EP
                
                
                Paratech','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/paratechnology-ep'),
 (11822,10,'Scytodes EP
                
                
                Barby','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/scytodes-ep'),
 (11823,10,'Slow Vibrations EP
                
                
                Solar Spectrum','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/slow-vibrations-ep'),
 (11824,10,'Waves EP
                
                
                Kyma','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/waves-ep'),
 (11825,10,'Beyond Universe EP
                
                
                Earthspace','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/beyond-universe-ep'),
 (11826,10,'Life Outside EP
                
                
                Mind Paradise','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/life-outside-ep'),
 (11827,10,'Expressions of One
                
                
                Via Axis','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/expressions-of-one'),
 (11828,10,'Texturas EP
                
                
                Gao','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/texturas-ep'),
 (11829,10,'Emotional EP
                
                
                Elegy','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/emotional-ep'),
 (11830,10,'Infinite Vibratory Levels EP
                
                
                BlueBliss','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/infinite-vibratory-levels-ep'),
 (11831,10,'Far Away From Home
                
                
                Frost Raven','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/far-away-from-home'),
 (11832,10,'Radio Telescope
                
                
                Predators','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/radio-telescope'),
 (11833,10,'Cosmogeny EP
                
                
                Ainur','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/cosmogeny-ep'),
 (11834,10,'Magnetic Portal
                
                
                Ovnimoon','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/magnetic-portal'),
 (11835,10,'Superstition EP
                
                
                Cosmo Circle','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/superstition-ep'),
 (11836,10,'Unknown Universe
                
                
                Cosmos Vibration','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/unknown-universe'),
 (11837,10,'Chile Psytrance vol. 3
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/chile-psytrance-vol-3'),
 (11838,10,'IV
                
                
                Wizack Twizack','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/iv'),
 (11839,10,'Echos EP
                
                
                Frost Raven','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/echos-ep'),
 (11840,10,'Spaceware
                
                
                Rigel','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/spaceware'),
 (11841,10,'Chile Psytrance vol. 2
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/chile-psytrance-vol-2'),
 (11842,10,'Polyhymnia
                
                
                Leenuz','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/polyhymnia'),
 (11843,10,'Audio Hustler EP
                
                
                Wizack Twizack','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/audio-hustler-ep'),
 (11844,10,'Doof Thing EP
                
                
                Audioform','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/doof-thing-ep'),
 (11845,10,'Alien Resistance EP
                
                
                Zybex','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/alien-resistance-ep'),
 (11846,10,'Convergence EP
                
                
                Man Machine','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/convergence-ep'),
 (11847,10,'Space Illusion EP
                
                
                Rigel','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/space-illusion-ep'),
 (11848,10,'Mysteries of Psytrance Vol. 1 - compiled by Ovnimoon
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/mysteries-of-psytrance-vol-1-compiled-by-ovnimoon'),
 (11849,10,'Wry Figments
                
                
                Psilocybian','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/wry-figments'),
 (11850,10,'Return Of Quetzalcoatl - Compiled by Ovnimoon &amp; Dr. Spook
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/return-of-quetzalcoatl-compiled-by-ovnimoon-dr-spook'),
 (11851,10,'ManMachine - Reintegrate (ovnicd003 / Ovnimoon Records)
                
                
                Man Machine','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/manmachine-reintegrate-ovnicd003-ovnimoon-records'),
 (11852,10,'Monte Mapu Festival by Ovnimoon
                
                
                Various Artists','2022-07-20',0,'/img/0.gif','https://ovnimoonrecords.bandcamp.com/album/monte-mapu-festival-by-ovnimoon'),
 (11853,11,'Rod (Electric Universe RMX)
                
                
                Tebra','2022-07-20',0,'https://f4.bcbits.com/img/a4056780445_2.jpg','https://electric-universe.bandcamp.com/track/rod-electric-universe-rmx'),
 (11854,11,'Arambolian Nights
                
                
                Electric Universe &amp; Hujaboy','2022-07-20',0,'https://f4.bcbits.com/img/a2502388274_2.jpg','https://electric-universe.bandcamp.com/track/arambolian-nights'),
 (11855,11,'Dub Stanza
                
                
                Electric Universe &amp; Sitarsonic','2022-07-20',0,'https://f4.bcbits.com/img/a0179802214_2.jpg','https://electric-universe.bandcamp.com/track/dub-stanza'),
 (11856,11,'Lakshmi','2022-07-20',0,'https://f4.bcbits.com/img/a1416401166_2.jpg','https://electric-universe.bandcamp.com/track/lakshmi'),
 (11857,11,'Dragonfly','2022-07-20',0,'https://f4.bcbits.com/img/a1380155717_2.jpg','https://electric-universe.bandcamp.com/track/dragonfly'),
 (11858,11,'Bansuri','2022-07-20',0,'https://f4.bcbits.com/img/a4022365282_2.jpg','https://electric-universe.bandcamp.com/track/bansuri'),
 (11859,11,'Mantra','2022-07-20',0,'https://f4.bcbits.com/img/a3602256610_2.jpg','https://electric-universe.bandcamp.com/track/mantra'),
 (11860,11,'Journeys Into Outer Space','2022-07-20',0,'https://f4.bcbits.com/img/a1685439143_2.jpg','https://electric-universe.bandcamp.com/album/journeys-into-outer-space'),
 (11861,11,'Mystical Experiences','2022-07-20',0,'/img/0.gif','https://electric-universe.bandcamp.com/album/mystical-experiences'),
 (11862,11,'Millenia','2022-07-20',0,'/img/0.gif','https://electric-universe.bandcamp.com/album/millenia'),
 (11863,11,'20','2022-07-20',0,'/img/0.gif','https://electric-universe.bandcamp.com/album/20'),
 (11864,11,'Ignition Sequence Start','2022-07-20',0,'/img/0.gif','https://electric-universe.bandcamp.com/track/ignition-sequence-start'),
 (11865,11,'Cosmic Experience','2022-07-20',0,'/img/0.gif','https://electric-universe.bandcamp.com/album/cosmic-experience'),
 (11866,11,'Symbolic - Insidius (Remix)','2022-07-20',0,'/img/0.gif','https://electric-universe.bandcamp.com/track/symbolic-insidius-remix'),
 (11867,11,'Stardiver','2022-07-20',0,'/img/0.gif','https://electric-universe.bandcamp.com/album/stardiver'),
 (11868,11,'One Love','2022-07-20',0,'/img/0.gif','https://electric-universe.bandcamp.com/album/one-love'),
 (11869,11,'Devine Design','2022-07-20',0,'/img/0.gif','https://electric-universe.bandcamp.com/album/devine-design'),
 (11870,11,'Gateway feat. Raja Ram','2022-07-20',0,'/img/0.gif','https://electric-universe.bandcamp.com/album/gateway-feat-raja-ram'),
 (11871,11,'Burning','2022-07-20',0,'/img/0.gif','https://electric-universe.bandcamp.com/album/burning'),
 (11872,11,'Higher Modes','2022-07-20',0,'/img/0.gif','https://electric-universe.bandcamp.com/album/higher-modes'),
 (11873,11,'Sonic Ecstasy','2022-07-20',0,'/img/0.gif','https://electric-universe.bandcamp.com/album/sonic-ecstasy'),
 (11874,11,'Silence In Action','2022-07-20',0,'/img/0.gif','https://electric-universe.bandcamp.com/album/silence-in-action'),
 (11875,11,'Blue Planet','2022-07-20',0,'/img/0.gif','https://electric-universe.bandcamp.com/album/blue-planet'),
 (11876,11,'Waves','2022-07-20',0,'/img/0.gif','https://electric-universe.bandcamp.com/album/waves'),
 (11877,12,'&quot;Inside the Upside Down&quot; E.P.
                
                
                Juno Reactor &amp; Eternal Basement','2022-07-20',0,'https://f4.bcbits.com/img/a1274046732_2.jpg','https://junoreactor.bandcamp.com/album/inside-the-upside-down-e-p'),
 (11878,12,'Navras Remix E.P.','2022-07-20',0,'https://f4.bcbits.com/img/a0943508013_2.jpg','https://junoreactor.bandcamp.com/album/navras-remix-e-p'),
 (11879,12,'The Mutant Theatre
                
                
                Juno Reactor','2022-07-20',0,'https://f4.bcbits.com/img/a4219006367_2.jpg','https://junoreactor.bandcamp.comhttps://junoreactor.bandcamp.com/album/the-mutant-theatre'),
 (11880,12,'The Golden Sun of the Great East
                
                
                Juno Reactor','2022-07-20',0,'https://f4.bcbits.com/img/a1394454392_2.jpg','https://junoreactor.bandcamp.comhttps://junoreactor.bandcamp.com/album/the-golden-sun-of-the-great-east'),
 (11881,12,'Gods &amp; Monsters
                
                
                Juno Reactor','2022-07-20',0,'https://f4.bcbits.com/img/a2550751364_2.jpg','https://junoreactor.bandcamp.comhttps://junoreactor.bandcamp.com/album/gods-monsters'),
 (11882,12,'Labyrinth
                
                
                Juno Reactor','2022-07-20',0,'https://f4.bcbits.com/img/a2174154688_2.jpg','https://junoreactor.bandcamp.comhttps://junoreactor.bandcamp.com/album/labyrinth'),
 (11883,12,'Shango
                
                
                Juno Reactor','2022-07-20',0,'https://f4.bcbits.com/img/a3996592874_2.jpg','https://junoreactor.bandcamp.comhttps://junoreactor.bandcamp.com/album/shango'),
 (11884,12,'Bible Of Dreams
                
                
                Juno Reactor','2022-07-20',0,'https://f4.bcbits.com/img/a3256122334_2.jpg','https://junoreactor.bandcamp.comhttps://junoreactor.bandcamp.com/album/bible-of-dreams'),
 (11885,12,'Beyond The Infinite','2022-07-20',0,'/img/0.gif','https://junoreactor.bandcamp.com/album/beyond-the-infinite'),
 (11886,12,'Transmissions
                
                
                Juno Reactor','2022-07-20',0,'/img/0.gif','https://junoreactor.bandcamp.comhttps://junoreactor.bandcamp.com/album/transmissions'),
 (11887,12,'The Golden Sun... Remixed
                
                
                Juno Reactor','2022-07-20',0,'/img/0.gif','https://junoreactor.bandcamp.comhttps://junoreactor.bandcamp.com/album/the-golden-sun-remixed'),
 (11888,12,'Inside The Reactor','2022-07-20',0,'/img/0.gif','https://junoreactor.bandcamp.com/album/inside-the-reactor'),
 (11889,12,'Into Valhalla
                
                
                Juno Reactor &amp; GMS','2022-07-20',0,'/img/0.gif','https://junoreactor.bandcamp.comhttps://junoreactor.bandcamp.com/album/into-valhalla'),
 (11890,12,'Odyssey 1992-2002
                
                
                Juno Reactor','2022-07-20',0,'/img/0.gif','https://junoreactor.bandcamp.comhttps://junoreactor.bandcamp.com/album/odyssey-1992-2002'),
 (11891,12,'Dakota
                
                
                Juno Reactor &amp; Undercover','2022-07-20',0,'/img/0.gif','https://junoreactor.bandcamp.comhttps://junoreactor.bandcamp.com/album/dakota'),
 (11892,12,'Our World
                
                
                Juno Reactor','2022-07-20',0,'/img/0.gif','https://junoreactor.bandcamp.comhttps://junoreactor.bandcamp.com/album/our-world'),
 (11893,12,'Final Frontier
                
                
                Juno Reactor','2022-07-20',0,'/img/0.gif','https://junoreactor.bandcamp.comhttps://junoreactor.bandcamp.com/album/final-frontier'),
 (11894,12,'Hotaka
                
                
                Juno Reactor','2022-07-20',0,'/img/0.gif','https://junoreactor.bandcamp.comhttps://junoreactor.bandcamp.com/album/hotaka'),
 (11895,12,'Masters Of The Universe
                
                
                Juno Reactor','2022-07-20',0,'/img/0.gif','https://junoreactor.bandcamp.comhttps://junoreactor.bandcamp.com/album/masters-of-the-universe'),
 (11896,12,'Pistolero
                
                
                Juno Reactor','2022-07-20',0,'/img/0.gif','https://junoreactor.bandcamp.comhttps://junoreactor.bandcamp.com/album/pistolero'),
 (11897,13,'Hold on your passion','2022-07-20',0,'https://f4.bcbits.com/img/a4232350580_2.jpg','https://talamascamusic.bandcamp.com/track/hold-on-your-passion'),
 (11898,13,'Alienergy','2022-07-20',0,'https://f4.bcbits.com/img/a2538054265_2.jpg','https://talamascamusic.bandcamp.com/track/alienergy'),
 (11899,13,'Lost worlds (feat. Nomad)
                
                
                Talamasca &amp; Nomad','2022-07-20',0,'https://f4.bcbits.com/img/a0480088177_2.jpg','https://talamascamusic.bandcamp.com/track/lost-worlds-feat-nomad'),
 (11900,13,'Mushroom robots','2022-07-20',0,'https://f4.bcbits.com/img/a1440047424_2.jpg','https://talamascamusic.bandcamp.com/track/mushroom-robots'),
 (11901,13,'A round reality','2022-07-20',0,'https://f4.bcbits.com/img/a3342721981_2.jpg','https://talamascamusic.bandcamp.com/track/a-round-reality'),
 (11902,13,'Winter tale chill rmx (feat. Ivan castro)','2022-07-20',0,'https://f4.bcbits.com/img/a1394232053_2.jpg','https://talamascamusic.bandcamp.com/track/winter-tale-chill-rmx-feat-ivan-castro'),
 (11903,13,'Exoplanet','2022-07-20',0,'https://f4.bcbits.com/img/a0055535382_2.jpg','https://talamascamusic.bandcamp.com/track/exoplanet'),
 (11904,13,'Jade sanctuary','2022-07-20',0,'https://f4.bcbits.com/img/a0867475778_2.jpg','https://talamascamusic.bandcamp.com/track/jade-sanctuary'),
 (11905,13,'Temple of lost dreams','2022-07-20',0,'/img/0.gif','https://talamascamusic.bandcamp.com/track/temple-of-lost-dreams'),
 (11906,13,'Healing plants (feat. Ivan castro)','2022-07-20',0,'/img/0.gif','https://talamascamusic.bandcamp.com/track/healing-plants-feat-ivan-castro'),
 (11907,13,'First contact','2022-07-20',0,'/img/0.gif','https://talamascamusic.bandcamp.com/track/first-contact'),
 (11908,13,'Old school for raver volume 3','2022-07-20',0,'/img/0.gif','https://talamascamusic.bandcamp.com/album/old-school-for-raver-volume-3'),
 (11909,13,'Old school for raver volume 2','2022-07-20',0,'/img/0.gif','https://talamascamusic.bandcamp.com/album/old-school-for-raver-volume-2'),
 (11910,13,'Old school for raver volume 1','2022-07-20',0,'/img/0.gif','https://talamascamusic.bandcamp.com/album/old-school-for-raver-volume-1'),
 (11911,13,'Sonic Massala : Champa (Talamasca rmx)','2022-07-20',0,'/img/0.gif','https://talamascamusic.bandcamp.com/track/sonic-massala-champa-talamasca-rmx'),
 (11912,13,'TALAMASCA : PANDEMIA','2022-07-20',0,'/img/0.gif','https://talamascamusic.bandcamp.com/track/talamasca-pandemia'),
 (11913,13,'Squazoid : Road to area 51 Talamasca &amp; Ivan castro remix. (Chill out / free style)','2022-07-20',0,'/img/0.gif','https://talamascamusic.bandcamp.com/track/squazoid-road-to-area-51-talamasca-ivan-castro-remix-chill-out-free-style'),
 (11914,13,'The experiment','2022-07-20',0,'/img/0.gif','https://talamascamusic.bandcamp.com/album/the-experiment'),
 (11915,13,'Positive visualisation','2022-07-20',0,'/img/0.gif','https://talamascamusic.bandcamp.com/track/positive-visualisation'),
 (11916,13,'Talamasca : Sensory Depravation','2022-07-20',0,'/img/0.gif','https://talamascamusic.bandcamp.com/track/talamasca-sensory-depravation'),
 (11917,13,'Talamasca &amp; Ivan Castro : We gonna rock the world','2022-07-20',0,'/img/0.gif','https://talamascamusic.bandcamp.com/album/talamasca-ivan-castro-we-gonna-rock-the-world'),
 (11918,13,'Talamasca &amp; Ivan Castro : Transition','2022-07-20',0,'/img/0.gif','https://talamascamusic.bandcamp.com/track/talamasca-ivan-castro-transition'),
 (11919,13,'A brief History of Goa-trance','2022-07-20',0,'/img/0.gif','https://talamascamusic.bandcamp.com/album/a-brief-history-of-goa-trance'),
 (11920,13,'TALAMASCA MASH UP.','2022-07-20',0,'/img/0.gif','https://talamascamusic.bandcamp.com/track/talamasca-mash-up'),
 (11921,13,'Talamasca and Ivan Castro : The beast remix','2022-07-20',0,'/img/0.gif','https://talamascamusic.bandcamp.com/track/talamasca-and-ivan-castro-the-beast-remix'),
 (11922,13,'The time machine','2022-07-20',0,'/img/0.gif','https://talamascamusic.bandcamp.com/album/the-time-machine'),
 (11923,13,'Level 9','2022-07-20',0,'/img/0.gif','https://talamascamusic.bandcamp.com/album/level-9'),
 (11924,13,'PSYCHEDELIC TRANCE','2022-07-20',0,'/img/0.gif','https://talamascamusic.bandcamp.com/album/psychedelic-trance'),
 (11925,13,'OBSESSIVE DREAM','2022-07-20',0,'/img/0.gif','https://talamascamusic.bandcamp.com/album/obsessive-dream'),
 (11926,13,'Make some noise','2022-07-20',0,'/img/0.gif','https://talamascamusic.bandcamp.com/album/make-some-noise'),
 (11927,13,'Made in Trance','2022-07-20',0,'/img/0.gif','https://talamascamusic.bandcamp.com/album/made-in-trance'),
 (11928,13,'Zodiac','2022-07-20',0,'/img/0.gif','https://talamascamusic.bandcamp.com/album/zodiac'),
 (11929,13,'Musica dvinorum','2022-07-20',0,'/img/0.gif','https://talamascamusic.bandcamp.com/album/musica-dvinorum'),
 (11930,14,'Zaida','2022-07-20',0,'https://f4.bcbits.com/img/a3604297594_2.jpg','https://gingersnap5.bandcamp.com/album/zaida'),
 (11931,14,'URUNWLCMHR','2022-07-20',0,'https://f4.bcbits.com/img/a0257769245_2.jpg','https://gingersnap5.bandcamp.com/album/urunwlcmhr'),
 (11932,14,'Ghosty','2022-07-20',0,'https://f4.bcbits.com/img/a0815944286_2.jpg','https://gingersnap5.bandcamp.com/album/ghosty'),
 (11933,14,'The Wrong Area','2022-07-20',0,'https://f4.bcbits.com/img/a2103465903_2.jpg','https://gingersnap5.bandcamp.com/album/the-wrong-area'),
 (11934,14,'Judgement Night','2022-07-20',0,'https://f4.bcbits.com/img/a0659372408_2.jpg','https://gingersnap5.bandcamp.com/album/judgement-night'),
 (11935,14,'Whitey','2022-07-20',0,'https://f4.bcbits.com/img/a0117969597_2.jpg','https://gingersnap5.bandcamp.com/album/whitey'),
 (11936,14,'The Girls Go Wild','2022-07-20',0,'https://f4.bcbits.com/img/a3466739110_2.jpg','https://gingersnap5.bandcamp.com/album/the-girls-go-wild'),
 (11937,14,'Make Me Bad','2022-07-20',0,'https://f4.bcbits.com/img/a0740423932_2.jpg','https://gingersnap5.bandcamp.com/album/make-me-bad'),
 (11938,14,'The Beasts Came To The Town (Deluxe)','2022-07-20',0,'/img/0.gif','https://gingersnap5.bandcamp.com/album/the-beasts-came-to-the-town-deluxe'),
 (11939,14,'The Beasts Came To The Town (Instrumental)','2022-07-20',0,'/img/0.gif','https://gingersnap5.bandcamp.com/album/the-beasts-came-to-the-town-instrumental'),
 (11940,14,'The Beasts Came To The Town (Extended)','2022-07-20',0,'/img/0.gif','https://gingersnap5.bandcamp.com/album/the-beasts-came-to-the-town-extended'),
 (11941,14,'The Beasts Came To The Town','2022-07-20',0,'/img/0.gif','https://gingersnap5.bandcamp.com/album/the-beasts-came-to-the-town'),
 (11942,14,'Dragon&#39;s Tail (Single Edit)','2022-07-20',0,'/img/0.gif','https://gingersnap5.bandcamp.com/track/dragons-tail-single-edit'),
 (11943,14,'The Pit (Single Edit)','2022-07-20',0,'/img/0.gif','https://gingersnap5.bandcamp.com/track/the-pit-single-edit'),
 (11944,14,'Urban Witches (Single Edit)','2022-07-20',0,'/img/0.gif','https://gingersnap5.bandcamp.com/track/urban-witches-single-edit'),
 (11945,14,'Wild Running (Single Edit)','2022-07-20',0,'/img/0.gif','https://gingersnap5.bandcamp.com/track/wild-running-single-edit'),
 (11946,14,'Drampage (Single Edit)','2022-07-20',0,'/img/0.gif','https://gingersnap5.bandcamp.com/track/drampage-single-edit'),
 (11947,14,'Enter the Action (Single Edit)','2022-07-20',0,'/img/0.gif','https://gingersnap5.bandcamp.com/track/enter-the-action-single-edit'),
 (11948,14,'The Light Between Us','2022-07-20',0,'/img/0.gif','https://gingersnap5.bandcamp.com/track/the-light-between-us'),
 (11949,14,'Insert Coin to Begin','2022-07-20',0,'/img/0.gif','https://gingersnap5.bandcamp.com/album/insert-coin-to-begin'),
 (11950,14,'Distant Star','2022-07-20',0,'/img/0.gif','https://gingersnap5.bandcamp.com/album/distant-star'),
 (11951,14,'Against The Days (Remastered)','2022-07-20',0,'/img/0.gif','https://gingersnap5.bandcamp.com/album/against-the-days-remastered'),
 (11952,14,'Against The Days','2022-07-20',0,'/img/0.gif','https://gingersnap5.bandcamp.com/album/against-the-days'),
 (11953,14,'Architect - Neon (remixed by Ginger Snap5)','2022-07-20',0,'/img/0.gif','https://gingersnap5.bandcamp.com/track/architect-neon-remixed-by-ginger-snap5'),
 (11954,14,'Heaven can wait (feat. Ulia Lord)','2022-07-20',0,'/img/0.gif','https://gingersnap5.bandcamp.com/track/heaven-can-wait-feat-ulia-lord'),
 (11955,14,'Polaris','2022-07-20',0,'/img/0.gif','https://gingersnap5.bandcamp.com/album/polaris'),
 (11956,14,'Snapped by you (instrumental)','2022-07-20',0,'/img/0.gif','https://gingersnap5.bandcamp.com/album/snapped-by-you-instrumental'),
 (11957,14,'Snapped By You (extended)','2022-07-20',0,'/img/0.gif','https://gingersnap5.bandcamp.com/album/snapped-by-you-extended'),
 (11958,14,'Break Me Down','2022-07-20',0,'/img/0.gif','https://gingersnap5.bandcamp.com/album/break-me-down'),
 (11959,14,'Linkin Park - A Light That Never Comes (touched by Ginger Snap5)','2022-07-20',0,'/img/0.gif','https://gingersnap5.bandcamp.com/track/linkin-park-a-light-that-never-comes-touched-by-ginger-snap5'),
 (11960,14,'Tears For Fears - Pale Shelter (touched by GINGER SNAP5)','2022-07-20',0,'/img/0.gif','https://gingersnap5.bandcamp.com/track/tears-for-fears-pale-shelter-touched-by-ginger-snap5'),
 (11961,15,'Chronosphere','2022-05-31 00:00:00',0,'https://41s.musify.club/img/68/25832425/64761415.jpg','https://musify.club/release/alphaxone-chronosphere-2019-1625922'),
 (11962,37,'Стальные Нервы CD3','2017-02-09 00:00:00',0,'https://41s.musify.club/img/69/12992246/33579623.jpg','https://musify.club/release/stalnie-nervi-cd3-2016-812763'),
 (11963,37,'God Is An Automaton','2012-09-09 00:00:00',0,'https://39s.musify.club/img/69/3919510/15423684.jpg','https://musify.club/release/sybreed-god-is-an-automaton-2012-326923'),
 (11964,37,'Power Metal','2019-08-10 00:00:00',0,'https://40s-a.musify.club/img/71/19334384/50108858.jpg','https://musify.club/release/power-metal-2012-1183902'),
 (11965,37,'Rosslyn Frequency - OST','2020-10-16 00:00:00',0,'https://38s.musify.club/img/68/22336360/57605623.jpg','https://musify.club/release/rosslyn-frequency-ost-2012-1389357'),
 (11966,37,'Fast Zombies - OST','2020-10-16 00:00:00',0,'https://38s.musify.club/img/68/22336356/57605616.jpg','https://musify.club/release/fast-zombies-ost-2012-1389356'),
 (11967,37,'Listenable Records Summer Free Sampler','2012-07-29 00:00:00',0,'https://38s-a.musify.club/img/70/3782506/41128151.jpg','https://musify.club/release/listenable-records-summer-free-sampler-2012-318923'),
 (11968,37,'Crushing Metal','2019-08-02 00:00:00',0,'https://40s-a.musify.club/img/71/19272265/49999142.jpg','https://musify.club/release/crushing-metal-2012-1180716'),
 (11969,37,'Challenger','2011-04-20 00:00:00',0,'https://41s-a.musify.club/img/70/1144191/40246304.jpg','https://musify.club/release/sybreed-challenger-2011-209354'),
 (11970,37,'Futuristic Metal Compilation: Cyber Metal','2011-12-06 00:00:00',0,'https://37s.musify.club/img/69/1809492/28560963.jpg','https://musify.club/release/futuristic-metal-compilation-cyber-metal-2011-262897'),
 (11971,37,'Всякое','2013-11-08 00:00:00',0,'https://39s-a.musify.club/img/70/5973058/18132312.jpg','https://musify.club/release/vsyakoe-2010-436996'),
 (11972,37,'The Pulse of Awakening','2009-10-17 00:00:00',0,'https://41s-a.musify.club/img/70/870602/40521003.jpg','https://musify.club/release/sybreed-the-pulse-of-awakening-2009-46658'),
 (11973,37,'A.E.O.N.','2009-09-29 00:00:00',0,'https://41s-a.musify.club/img/70/862835/40529388.jpg','https://musify.club/release/sybreed-a-e-o-n-2009-44064'),
 (11974,37,'Antares','2007-10-09 00:00:00',0,'https://40s.musify.club/img/69/705466/13320753.jpg','https://musify.club/release/sybreed-antares-2007-5381'),
 (11975,37,'Slave Design','2008-03-19 00:00:00',0,'https://39s.musify.club/img/69/4672124/15337923.jpg','https://musify.club/release/sybreed-slave-design-2004-12036'),
 (11978,37,'3 Songs Promo','2015-05-01 00:00:00',0,'https://39s.musify.club/img/69/9353174/24932579.jpg','https://musify.club/release/sybreed-3-songs-promo-2003-617179'),
 (11979,37,'Bioactive EP','2015-05-01 00:00:00',0,'https://39s.musify.club/img/69/9353104/24932568.jpg','https://musify.club/release/sybreed-bioactive-ep-2003-617180');
INSERT INTO Tracks VALUES (1579,9821,'Ectogenesis','https://musify.club/track/dl/16266212/neurotech-ectogenesis.mp3'),
 (1596,9604,'The Cosmic Wilderness','https://musify.club/track/dl/16101732/melodysheep-the-cosmic-wilderness.mp3'),
 (1597,9604,'Possibilities','https://musify.club/track/dl/16101733/melodysheep-possibilities.mp3'),
 (1598,9604,'Brethren','https://musify.club/track/dl/16101734/melodysheep-brethren.mp3'),
 (1599,9604,'Convergence','https://musify.club/track/dl/16101735/melodysheep-convergence.mp3'),
 (1600,9604,'Replica K','https://musify.club/track/dl/16101736/melodysheep-replica-k.mp3'),
 (1601,9604,'Distant Worlds','https://musify.club/track/dl/16101737/melodysheep-distant-worlds.mp3'),
 (1602,9604,'Takara','https://musify.club/track/dl/16101738/melodysheep-takara.mp3'),
 (1603,9604,'Invisibilia','https://musify.club/track/dl/16101739/melodysheep-invisibilia.mp3'),
 (1604,9604,'Signals','https://musify.club/track/dl/16101740/melodysheep-signals.mp3'),
 (1605,9604,'Behemoth','https://musify.club/track/dl/16101741/melodysheep-behemoth.mp3'),
 (1606,9604,'Incognitum','https://musify.club/track/dl/16101742/melodysheep-incognitum.mp3'),
 (1607,9604,'The Mind Of Nature','https://musify.club/track/dl/16101743/melodysheep-the-mind-of-nature.mp3'),
 (1608,9604,'Life Dust','https://musify.club/track/dl/16101744/melodysheep-life-dust.mp3'),
 (1609,9604,'Strange Sea','https://musify.club/track/dl/16101745/melodysheep-strange-sea.mp3'),
 (1610,9604,'First Link','https://musify.club/track/dl/16101746/melodysheep-first-link.mp3'),
 (1611,9604,'Go Looking','https://musify.club/track/dl/16101747/melodysheep-go-looking.mp3'),
 (1612,9604,'Cascadia','https://musify.club/track/dl/16101748/melodysheep-cascadia.mp3'),
 (1613,8866,'The Conjurer','https://musify.club/track/dl/17172981/insomnium-the-conjurer.mp3'),
 (1614,8866,'The Reticent','https://musify.club/track/dl/17172982/insomnium-the-reticent.mp3'),
 (1615,8866,'The Antagonist','https://musify.club/track/dl/17172983/insomnium-the-antagonist.mp3'),
 (1616,8866,'The Wanderer','https://musify.club/track/dl/17172984/insomnium-the-wanderer.mp3'),
 (1617,11961,'Reverse Echoes','https://musify.club/track/dl/18061572/alphaxone-reverse-echoes.mp3'),
 (1618,11961,'Delta Zero','https://musify.club/track/dl/18061573/alphaxone-delta-zero.mp3'),
 (1619,11961,'Spellbound','https://musify.club/track/dl/18061574/alphaxone-spellbound.mp3'),
 (1620,11961,'Into The Void','https://musify.club/track/dl/18061575/alphaxone-into-the-void.mp3'),
 (1621,11961,'Parallel Destiny','https://musify.club/track/dl/18061576/alphaxone-parallel-destiny.mp3'),
 (1622,11961,'Distance Experience','https://musify.club/track/dl/18061577/alphaxone-distance-experience.mp3'),
 (1623,11961,'Ancient Pillars','https://musify.club/track/dl/18061578/alphaxone-ancient-pillars.mp3'),
 (1624,11961,'Particle Storm','https://musify.club/track/dl/18061579/alphaxone-particle-storm.mp3'),
 (1625,11961,'Floating Spheres','https://musify.club/track/dl/18061580/alphaxone-floating-spheres.mp3'),
 (1626,8603,'Ground Exploration','https://musify.club/track/dl/17321653/alphaxone-ground-exploration.mp3'),
 (1627,8603,'Space Exploration','https://musify.club/track/dl/17321654/alphaxone-space-exploration.mp3'),
 (1628,8602,'Derelict','https://musify.club/track/dl/17316094/alphaxone-derelict.mp3'),
 (1629,8602,'Experience','https://musify.club/track/dl/17316095/alphaxone-experience.mp3'),
 (1630,8602,'Compression','https://musify.club/track/dl/17316096/alphaxone-compression.mp3'),
 (1631,8602,'Substance','https://musify.club/track/dl/17316097/alphaxone-substance.mp3'),
 (1632,8602,'Dissociative','https://musify.club/track/dl/17316098/alphaxone-dissociative.mp3'),
 (1633,8602,'Involvement','https://musify.club/track/dl/17316099/alphaxone-involvement.mp3'),
 (1634,8602,'Underneath','https://musify.club/track/dl/17316100/alphaxone-underneath.mp3'),
 (1635,8602,'Aftershock','https://musify.club/track/dl/17316101/alphaxone-aftershock.mp3'),
 (1636,8602,'Dissolution','https://musify.club/track/dl/17316102/alphaxone-dissolution.mp3'),
 (1637,8608,'Lost In The Moment','https://musify.club/track/dl/14138059/alphaxone-lost-in-the-moment.mp3'),
 (1638,8608,'Falling Time','https://musify.club/track/dl/14138060/alphaxone-falling-time.mp3'),
 (1639,8608,'Out Of Source','https://musify.club/track/dl/14138061/alphaxone-out-of-source.mp3'),
 (1640,8608,'Frozen Light','https://musify.club/track/dl/14138062/alphaxone-frozen-light.mp3'),
 (1641,8608,'Mysterium','https://musify.club/track/dl/14138063/alphaxone-mysterium.mp3'),
 (1642,8608,'Hollow Lands','https://musify.club/track/dl/14138064/alphaxone-hollow-lands.mp3'),
 (1643,8608,'Dissolving Horizon','https://musify.club/track/dl/14138065/alphaxone-dissolving-horizon.mp3'),
 (1644,8608,'Underworld','https://musify.club/track/dl/14138066/alphaxone-underworld.mp3'),
 (1645,8608,'A Profound Void','https://musify.club/track/dl/14138067/alphaxone-a-profound-void.mp3'),
 (1646,8633,'Elements','https://musify.club/track/dl/2724999/alphaxone-elements.mp3'),
 (1647,8633,'Primary Genesis','https://musify.club/track/dl/2725000/alphaxone-primary-genesis.mp3'),
 (1648,8633,'Above All','https://musify.club/track/dl/2725001/alphaxone-above-all.mp3'),
 (1649,8633,'Below Atmos','https://musify.club/track/dl/2725002/alphaxone-below-atmos.mp3'),
 (1650,8633,'Turning Point','https://musify.club/track/dl/2725003/alphaxone-turning-point.mp3'),
 (1651,8633,'Extended','https://musify.club/track/dl/2725004/alphaxone-extended.mp3'),
 (1652,8633,'Transform','https://musify.club/track/dl/2725005/alphaxone-transform.mp3'),
 (1653,8633,'Synthetic Vision','https://musify.club/track/dl/2725006/alphaxone-synthetic-vision.mp3'),
 (1654,9807,'Deceive','https://musify.club/track/dl/17744395/neurotech-deceive.mp3'),
 (1655,9806,'The Prophetic Symphony','https://musify.club/track/dl/17744391/neurotech-the-prophetic-symphony.mp3'),
 (1656,9806,'The Seraphic Symphony','https://musify.club/track/dl/17744392/neurotech-the-seraphic-symphony.mp3'),
 (1657,9806,'The Draconic Symphony','https://musify.club/track/dl/17744393/neurotech-the-draconic-symphony.mp3'),
 (1658,9806,'The Messianic Symphony','https://musify.club/track/dl/17744394/neurotech-the-messianic-symphony.mp3'),
 (1659,11972,'Nomenklatura','https://musify.club/track/dl/570570/sybreed-nomenklatura.mp3'),
 (1660,11972,'A.E.O.N.','https://musify.club/track/dl/570571/sybreed-a-e-o-n.mp3'),
 (1661,11972,'Doomsday Party','https://musify.club/track/dl/570572/sybreed-doomsday-party.mp3'),
 (1662,11972,'Human Black Box','https://musify.club/track/dl/570573/sybreed-human-black-box.mp3'),
 (1663,11972,'Kill Joy','https://musify.club/track/dl/570574/sybreed-kill-joy.mp3'),
 (1664,11972,'I Am Ulraviolence','https://musify.club/track/dl/570575/sybreed-i-am-ulraviolence.mp3'),
 (1665,11972,'Electronegative','https://musify.club/track/dl/570576/sybreed-electronegative.mp3'),
 (1666,11972,'In The Cold Light','https://musify.club/track/dl/570577/sybreed-in-the-cold-light.mp3'),
 (1667,11972,'Lucifer Effect','https://musify.club/track/dl/570578/sybreed-lucifer-effect.mp3'),
 (1668,11972,'Love Like Blood','https://musify.club/track/dl/570579/sybreed-love-like-blood.mp3'),
 (1669,11972,'Meridian A.D.','https://musify.club/track/dl/570580/sybreed-meridian-a-d.mp3'),
 (1670,11972,'Flesh Doll For Sale (Japanese Bonus Track)','https://musify.club/track/dl/2708484/sybreed-flesh-doll-for-sale-japanese-bonus-track.mp3'),
 (1671,11972,'From Zero To Nothing','https://musify.club/track/dl/570581/sybreed-from-zero-to-nothing.mp3'),
 (1672,11973,'A.E.O.N.','https://musify.club/track/dl/545471/sybreed-a-e-o-n.mp3'),
 (1673,11973,'Human Black Box','https://musify.club/track/dl/545472/sybreed-human-black-box.mp3'),
 (1674,11975,'Bioactive','https://musify.club/track/dl/163879/sybreed-bioactive.mp3'),
 (1675,11975,'ReEvolution','https://musify.club/track/dl/163880/sybreed-reevolution.mp3'),
 (1676,11975,'Decoy','https://musify.club/track/dl/163881/sybreed-decoy.mp3'),
 (1677,11975,'Sinthetic Breed','https://musify.club/track/dl/163877/sybreed-sinthetic-breed.mp3'),
 (1678,11975,'Next Day Will Never Come','https://musify.club/track/dl/163872/sybreed-next-day-will-never-come.mp3'),
 (1679,11975,'Machine Gun Messiah','https://musify.club/track/dl/163878/sybreed-machine-gun-messiah.mp3'),
 (1680,11975,'Take The Red Pill','https://musify.club/track/dl/163875/sybreed-take-the-red-pill.mp3'),
 (1681,11975,'Rusted','https://musify.club/track/dl/163874/sybreed-rusted.mp3'),
 (1682,11975,'Static Currents','https://musify.club/track/dl/163876/sybreed-static-currents.mp3'),
 (1683,11975,'Critical Mass','https://musify.club/track/dl/163873/sybreed-critical-mass.mp3'),
 (1684,11975,'Reevolution (Syntax Airplay Edit)','https://musify.club/track/dl/163871/sybreed-reevolution-syntax-airplay-edit.mp3'),
 (1685,11975,'Decoy (Radio Slave Edit)','https://musify.club/track/dl/163870/sybreed-decoy-radio-slave-edit.mp3'),
 (1686,11963,'Posthuman Manifesto','https://musify.club/track/dl/2684487/sybreed-posthuman-manifesto.mp3'),
 (1687,11963,'No Wisdom Brings Solace','https://musify.club/track/dl/2684488/sybreed-no-wisdom-brings-solace.mp3'),
 (1688,11963,'The Line Of Least Resistance','https://musify.club/track/dl/2684489/sybreed-the-line-of-least-resistance.mp3'),
 (1689,11963,'Red Nova Ignition','https://musify.club/track/dl/2684490/sybreed-red-nova-ignition.mp3'),
 (1690,11963,'God Is An Automaton','https://musify.club/track/dl/2684491/sybreed-god-is-an-automaton.mp3'),
 (1691,11963,'Hightech Versus Lowlife','https://musify.club/track/dl/2684492/sybreed-hightech-versus-lowlife.mp3'),
 (1692,11963,'Downfall Inc','https://musify.club/track/dl/2684493/sybreed-downfall-inc.mp3'),
 (1693,11963,'Challenger','https://musify.club/track/dl/2684494/sybreed-challenger.mp3'),
 (1694,11963,'A Radiant Daybreak','https://musify.club/track/dl/2684496/sybreed-a-radiant-daybreak.mp3'),
 (1695,11963,'Into The Blackest Light','https://musify.club/track/dl/2684495/sybreed-into-the-blackest-light.mp3'),
 (1696,11963,'Destruction And Bliss','https://musify.club/track/dl/2684497/sybreed-destruction-and-bliss.mp3'),
 (1697,9650,'Terraformer','https://musify.club/track/dl/7910970/centavra-project-terraformer.mp3'),
 (1698,9650,'Words of Wisdom','https://musify.club/track/dl/7910971/celestial-intelligence-words-of-wisdom.mp3'),
 (1699,9650,'Night Train','https://musify.club/track/dl/7910972/alienapia-night-train.mp3'),
 (1700,9650,'Strange Object','https://musify.club/track/dl/7910973/centavra-project-strange-object.mp3'),
 (1701,9650,'The Essence of Bhagavat-Gita','https://musify.club/track/dl/7910974/psy-h-project-the-essence-of-bhagavat-gita.mp3'),
 (1702,9650,'Are You Mad','https://musify.club/track/dl/7910975/alienapia-are-you-mad.mp3'),
 (1703,9650,'Enchanted Land','https://musify.club/track/dl/7910976/mindsphere-enchanted-land.mp3'),
 (1704,9650,'Fucked Up Beyond Repair','https://musify.club/track/dl/7910977/javi-and-sko0ma-fucked-up-beyond-repair.mp3'),
 (1705,9650,'Pangea','https://musify.club/track/dl/7910978/omnivox-pangea.mp3'),
 (1706,9650,'Edge of Infinity (Part 1)','https://musify.club/track/dl/7910979/sykespico-edge-of-infinity-part-1.mp3'),
 (1707,9650,'Mad Space','https://musify.club/track/dl/7910980/median-project-mad-space.mp3'),
 (1708,9650,'Sacred Mantra','https://musify.club/track/dl/7910981/atlantis-greece-sacred-mantra.mp3'),
 (1709,9650,'Searching for UFOs [Median Project Remix]','https://musify.club/track/dl/7910982/fiery-dawn-searching-for-ufos-median-project-remix.mp3'),
 (1710,9650,'Conquer the Universe','https://musify.club/track/dl/7910983/clementz-conquer-the-universe.mp3'),
 (1711,9650,'Indian Walk','https://musify.club/track/dl/7910984/omneon-indian-walk.mp3'),
 (1712,9650,'A Journey Out of Your Mind','https://musify.club/track/dl/7910985/atlantis-greece-a-journey-out-of-your-mind.mp3'),
 (1713,9650,'The Third Element','https://musify.club/track/dl/7910986/median-project-the-third-element.mp3'),
 (1714,9650,'Cianendeon','https://musify.club/track/dl/7910987/skarma-cianendeon.mp3'),
 (1715,9650,'Vibrating Universe','https://musify.club/track/dl/7910988/fiery-dawn-vibrating-universe.mp3'),
 (1716,9650,'Message to God','https://musify.club/track/dl/7910989/zopmanika-message-to-god.mp3'),
 (1717,9650,'Ascension','https://musify.club/track/dl/7910990/raving-universe-ascension.mp3'),
 (1718,9650,'Gaura Nitay','https://musify.club/track/dl/7910991/psy-h-project-gaura-nitay.mp3'),
 (1719,9650,'Journey to Another Worlds (Mix 2017)','https://musify.club/track/dl/7910992/centavra-project-journey-to-another-worlds-mix-2017.mp3'),
 (1720,9650,'Space Mantra','https://musify.club/track/dl/7910993/atlantis-greece-space-mantra.mp3'),
 (1721,9650,'Posessed [Cosmic Dimension Remix]','https://musify.club/track/dl/7910994/man-with-no-name-posessed-cosmic-dimension-remix.mp3'),
 (1722,9650,'Hidden Depth','https://musify.club/track/dl/7910995/mindsphere-hidden-depth.mp3'),
 (1723,9650,'Contact','https://musify.club/track/dl/7910996/artifact303-contact.mp3'),
 (1724,9650,'Krishna or Maya','https://musify.club/track/dl/7910997/psy-h-project-krishna-or-maya.mp3'),
 (1725,9650,'Magma','https://musify.club/track/dl/7910998/fiery-dawn-magma.mp3'),
 (1726,9650,'Event Horizon','https://musify.club/track/dl/7910999/liquid-flow-event-horizon.mp3'),
 (1727,9650,'Evolutionary Stream','https://musify.club/track/dl/7911000/katedra-evolutionary-stream.mp3'),
 (1728,9650,'Starlight','https://musify.club/track/dl/7911001/fiery-dawn-starlight.mp3'),
 (1729,9650,'In a Rain Drop','https://musify.club/track/dl/7911002/slow-reflections-in-a-rain-drop.mp3'),
 (1730,9650,'Delirium (Downward Spiral)','https://musify.club/track/dl/7911003/artifact303-delirium-downward-spiral.mp3'),
 (1731,9655,'Krishna or Maya','https://musify.club/track/dl/5698868/psy-h-project-krishna-or-maya.mp3'),
 (1732,9655,'Outro (Extended Version)','https://musify.club/track/dl/5698869/morphic-resonance-outro-extended-version.mp3'),
 (1733,9655,'Event Horizon','https://musify.club/track/dl/5698870/liquid-flow-event-horizon.mp3'),
 (1734,9655,'Hidden Valley','https://musify.club/track/dl/5698871/celestial-intelligence-hidden-valley.mp3'),
 (1735,9655,'Delirium (Downward Spiral)','https://musify.club/track/dl/5698872/artifact303-delirium-downward-spiral.mp3'),
 (1736,9657,'Spiritual Experience','https://musify.club/track/dl/2816249/psy-h-project-spiritual-experience.mp3'),
 (1737,9657,'Psychopath','https://musify.club/track/dl/2816250/radical-distortion-psychopath.mp3'),
 (1738,9657,'Tesla (Original Mix)','https://musify.club/track/dl/2816251/travma-tesla-original-mix.mp3'),
 (1739,9657,'Morning Venus','https://musify.club/track/dl/2816252/asirion-morning-venus.mp3'),
 (1740,9657,'The Answer','https://musify.club/track/dl/2816253/artifact303-the-answer.mp3'),
 (1741,9657,'Visual Perception','https://musify.club/track/dl/2816254/mindsphere-visual-perception.mp3'),
 (1742,9657,'Holographic Universe','https://musify.club/track/dl/2816255/artifact303-holographic-universe.mp3'),
 (1743,9657,'Celestial Alignements','https://musify.club/track/dl/2816256/e-mantra-celestial-alignements.mp3'),
 (1744,9657,'What If I Can Fly?','https://musify.club/track/dl/2816257/armageddance-what-if-i-can-fly.mp3'),
 (1745,9653,'Crystal Worlds','https://musify.club/track/dl/5959912/psy-h-project-crystal-worlds.mp3'),
 (1746,9653,'Intergalactic','https://musify.club/track/dl/5959913/sirius-goa-trance-intergalactic.mp3'),
 (1747,9653,'Nyad Of The Infinite Sea','https://musify.club/track/dl/5959914/filteria-nyad-of-the-infinite-sea.mp3'),
 (1748,9653,'Infinity','https://musify.club/track/dl/5959915/celestial-intelligence-infinity.mp3'),
 (1749,9653,'Life Support System','https://musify.club/track/dl/5959916/artifact303-life-support-system.mp3'),
 (1750,9653,'Animoneae','https://musify.club/track/dl/5959917/skarma-animoneae.mp3'),
 (1751,9653,'Capsula','https://musify.club/track/dl/5959918/centavra-project-capsula.mp3'),
 (1752,9653,'Astral Travellers','https://musify.club/track/dl/5959919/somnesia-imba-astral-travellers.mp3'),
 (1753,9653,'Born Of Osiris','https://musify.club/track/dl/5959920/zirrex-born-of-osiris.mp3'),
 (1754,9653,'Dark Matter','https://musify.club/track/dl/5959921/psy-h-project-dark-matter.mp3'),
 (1755,9653,'Brahamantra','https://musify.club/track/dl/5959922/kurandini-brahamantra.mp3'),
 (1756,9653,'Radiation','https://musify.club/track/dl/5959923/liquid-flow-radiation.mp3'),
 (1757,9653,'Future Power','https://musify.club/track/dl/5959924/artifact303-future-power.mp3'),
 (1758,9653,'Moonwalker','https://musify.club/track/dl/5959925/morphic-resonance-moonwalker.mp3'),
 (1759,9653,'Crystal Gazer','https://musify.club/track/dl/5959926/celestial-intelligence-crystal-gazer.mp3'),
 (1760,9653,'Black Light','https://musify.club/track/dl/5959927/artifact303-black-light.mp3'),
 (1761,9653,'Radiointerference','https://musify.club/track/dl/5959928/katedra-radiointerference.mp3'),
 (1762,9653,'Endless Glade','https://musify.club/track/dl/5959929/khetzal-alienapia-endless-glade.mp3'),
 (1763,9653,'Minding Of The Universe','https://musify.club/track/dl/5959930/celestial-intelligence-minding-of-the-universe.mp3'),
 (1764,9653,'Precession Of The Universe','https://musify.club/track/dl/5959931/psy-h-project-precession-of-the-universe.mp3'),
 (1765,9653,'Burning Universe','https://musify.club/track/dl/5959932/merrow-burning-universe.mp3'),
 (1766,9653,'Stargate','https://musify.club/track/dl/5959933/nova-fractal-e-mantra-oxi-stargate.mp3'),
 (1767,9653,'Galactic System','https://musify.club/track/dl/5959934/centavra-project-galactic-system.mp3'),
 (1768,9653,'Divine Intervention','https://musify.club/track/dl/5959935/mindsphere-divine-intervention.mp3'),
 (1769,9653,'Tropical Sunset (Trance Dance Remix)','https://musify.club/track/dl/5959936/artifact303-tropical-sunset-trance-dance-remix.mp3'),
 (1770,9653,'Brahma Samhita','https://musify.club/track/dl/5959937/psy-h-project-brahma-samhita.mp3'),
 (1771,9653,'Family Of Light','https://musify.club/track/dl/5959938/artifact303-family-of-light.mp3'),
 (1772,9656,'Existence Of The Universe','https://musify.club/track/dl/2816239/slow-reflections-existence-of-the-universe.mp3'),
 (1773,9656,'Exhalation Of Mahavishnu','https://musify.club/track/dl/2816240/psy-h-project-exhalation-of-mahavishnu.mp3'),
 (1774,9656,'Dark Corners Of The Universe','https://musify.club/track/dl/2816241/e-mantra-dark-corners-of-the-universe.mp3'),
 (1775,9656,'Psyonic Storm','https://musify.club/track/dl/2816242/liquid-flow-psyonic-storm.mp3'),
 (1776,9656,'In Your Mind','https://musify.club/track/dl/2816243/artifact303-in-your-mind.mp3'),
 (1777,9656,'Dreams Are Under Construction','https://musify.club/track/dl/2816244/filteria-dreams-are-under-construction.mp3'),
 (1778,9656,'Metempsychosis (Remix)','https://musify.club/track/dl/2816245/e-mantra-metempsychosis-remix.mp3'),
 (1779,9656,'Chaotic','https://musify.club/track/dl/2816246/liquid-flow-chaotic.mp3'),
 (1780,9656,'The Religion','https://musify.club/track/dl/2816247/goasia-the-religion.mp3'),
 (1781,9656,'Solioonensius','https://musify.club/track/dl/2816248/astrancer-solioonensius.mp3'),
 (1782,9647,'Terraformer','https://musify.club/track/dl/7691427/centavra-project-terraformer.mp3'),
 (1783,9647,'Words of Wisdom','https://musify.club/track/dl/7691428/celestial-intelligence-words-of-wisdom.mp3'),
 (1784,9647,'Night Train','https://musify.club/track/dl/7691429/alienapia-night-train.mp3'),
 (1785,9647,'Strange Object','https://musify.club/track/dl/7691430/centavra-project-strange-object.mp3'),
 (1786,9647,'Conquer the Universe','https://musify.club/track/dl/7691431/clementz-conquer-the-universe.mp3'),
 (1787,9647,'Gaura Nitay','https://musify.club/track/dl/7691432/psy-h-project-gaura-nitay.mp3'),
 (1788,9570,'Through The Old Mirror','https://musify.club/track/dl/15131658/god-body-disconnect-through-the-old-mirror.mp3'),
 (1789,9570,'I Can Feel Who You Were','https://musify.club/track/dl/15131659/god-body-disconnect-i-can-feel-who-you-were.mp3'),
 (1790,9570,'Every Forest Has Its Shadow','https://musify.club/track/dl/15131660/god-body-disconnect-every-forest-has-its-shadow.mp3'),
 (1791,9570,'Floating Among The Sleepers','https://musify.club/track/dl/15131661/god-body-disconnect-floating-among-the-sleepers.mp3'),
 (1792,9570,'The Fear Of Deliverance','https://musify.club/track/dl/15131662/god-body-disconnect-the-fear-of-deliverance.mp3'),
 (1793,9570,'Dreaming Between The Trees','https://musify.club/track/dl/15131663/god-body-disconnect-dreaming-between-the-trees.mp3'),
 (1794,9570,'At The End Of A Breath','https://musify.club/track/dl/15131664/god-body-disconnect-at-the-end-of-a-breath.mp3'),
 (1795,9570,'We All Turn To Stars','https://musify.club/track/dl/15131665/god-body-disconnect-we-all-turn-to-stars.mp3'),
 (1796,9570,'Through The Old Mirror','https://musify.club/track/dl/15131658/god-body-disconnect-through-the-old-mirror.mp3'),
 (1797,9570,'I Can Feel Who You Were','https://musify.club/track/dl/15131659/god-body-disconnect-i-can-feel-who-you-were.mp3'),
 (1798,9570,'Every Forest Has Its Shadow','https://musify.club/track/dl/15131660/god-body-disconnect-every-forest-has-its-shadow.mp3'),
 (1799,9570,'Floating Among The Sleepers','https://musify.club/track/dl/15131661/god-body-disconnect-floating-among-the-sleepers.mp3'),
 (1800,9570,'The Fear Of Deliverance','https://musify.club/track/dl/15131662/god-body-disconnect-the-fear-of-deliverance.mp3'),
 (1801,9570,'Dreaming Between The Trees','https://musify.club/track/dl/15131663/god-body-disconnect-dreaming-between-the-trees.mp3'),
 (1802,9570,'At The End Of A Breath','https://musify.club/track/dl/15131664/god-body-disconnect-at-the-end-of-a-breath.mp3'),
 (1803,9570,'We All Turn To Stars','https://musify.club/track/dl/15131665/god-body-disconnect-we-all-turn-to-stars.mp3'),
 (1804,9815,'Hope','https://musify.club/track/dl/16266214/neurotech-hope.mp3'),
 (1805,9815,'A View Of The Troposphere','https://musify.club/track/dl/16266215/neurotech-a-view-of-the-troposphere.mp3'),
 (1806,9815,'Hypnotic','https://musify.club/track/dl/16266216/neurotech-hypnotic.mp3'),
 (1807,9815,'In The Pit','https://musify.club/track/dl/16266217/neurotech-in-the-pit.mp3'),
 (1808,9815,'Polar Vortex','https://musify.club/track/dl/16266218/neurotech-polar-vortex.mp3'),
 (1809,8880,'While We Sleep','https://musify.club/track/dl/4148748/insomnium-while-we-sleep.mp3'),
 (1810,8879,'The Primeval Dark','https://musify.club/track/dl/4193817/insomnium-the-primeval-dark.mp3'),
 (1811,8879,'While We Sleep','https://musify.club/track/dl/4193818/insomnium-while-we-sleep.mp3'),
 (1812,8879,'Revelation','https://musify.club/track/dl/4193819/insomnium-revelation.mp3'),
 (1813,8879,'Black Heart Rebellion','https://musify.club/track/dl/4193820/insomnium-black-heart-rebellion.mp3'),
 (1814,8879,'Lose To Night','https://musify.club/track/dl/4193821/insomnium-lose-to-night.mp3'),
 (1815,8879,'Collapsing Words','https://musify.club/track/dl/4193822/insomnium-collapsing-words.mp3'),
 (1816,8879,'The River','https://musify.club/track/dl/4193823/insomnium-the-river.mp3'),
 (1817,8879,'Ephemeral (Album Version)','https://musify.club/track/dl/4193824/insomnium-ephemeral-album-version.mp3'),
 (1818,8879,'The Promethean Song','https://musify.club/track/dl/4193825/insomnium-the-promethean-song.mp3'),
 (1819,8879,'Shadows Of The Dying Sun','https://musify.club/track/dl/4193826/insomnium-shadows-of-the-dying-sun.mp3'),
 (1820,8879,'Out To The Sea','https://musify.club/track/dl/4222426/insomnium-out-to-the-sea.mp3'),
 (1821,8879,'The Emergence','https://musify.club/track/dl/4222424/insomnium-the-emergence.mp3'),
 (1822,8879,'The Swarm','https://musify.club/track/dl/4222425/insomnium-the-swarm.mp3'),
 (1823,8879,'The Descent','https://musify.club/track/dl/4222427/insomnium-the-descent.mp3'),
 (1824,8889,'Nach Asgard Wir Reiten','https://musify.club/track/dl/2019209/obscurity-nach-asgard-wir-reiten.mp3'),
 (1825,8889,'Gryende Tidevarv','https://musify.club/track/dl/2019210/fimbultyr-gryende-tidevarv.mp3'),
 (1826,8889,'Claw The Clouds','https://musify.club/track/dl/2019211/satariel-claw-the-clouds.mp3'),
 (1827,8889,'Endtime Divine','https://musify.club/track/dl/2019212/setherial-endtime-divine.mp3'),
 (1828,8889,'Finally The World Shall Shape','https://musify.club/track/dl/2019213/svarttjern-finally-the-world-shall-shape.mp3'),
 (1829,8889,'Laukr','https://musify.club/track/dl/2019214/wardruna-laukr.mp3'),
 (1830,8889,'Wildfire Season','https://musify.club/track/dl/2019215/crimfall-wildfire-season.mp3'),
 (1831,8889,'Under The Mighty Oath','https://musify.club/track/dl/2019216/hammer-horde-under-the-mighty-oath.mp3'),
 (1832,8889,'The Mystory','https://musify.club/track/dl/2019217/vesania-the-mystory.mp3'),
 (1833,8889,'Isle Of Skye','https://musify.club/track/dl/2019218/suidakra-isle-of-skye.mp3'),
 (1834,8889,'Mortal Share','https://musify.club/track/dl/2019219/insomnium-mortal-share.mp3'),
 (1835,8889,'Akramos','https://musify.club/track/dl/2019220/shambless-akramos.mp3'),
 (1836,8889,'Wings Of Fire','https://musify.club'),
 (1837,8889,'Soldnerschwein','https://musify.club/track/dl/2019222/black-messiah-soldnerschwein.mp3'),
 (1838,8889,'Sands Of Hinnom','https://musify.club/track/dl/2019223/gloria-morti-sands-of-hinnom.mp3'),
 (1839,8889,'Dark Clouds','https://musify.club/track/dl/2019224/isole-dark-clouds.mp3'),
 (1840,8889,'I Saw Hell','https://musify.club/track/dl/2019225/hellsaw-i-saw-hell.mp3'),
 (1841,8889,'Secret Coldness','https://musify.club/track/dl/2019226/deception-secret-coldness.mp3'),
 (1842,8882,'Ephemeral','https://musify.club/track/dl/3525333/insomnium-ephemeral.mp3'),
 (1843,8882,'The Emergence','https://musify.club/track/dl/3525334/insomnium-the-emergence.mp3'),
 (1844,8882,'The Swarm','https://musify.club/track/dl/3525335/insomnium-the-swarm.mp3'),
 (1845,8882,'The Descent','https://musify.club/track/dl/3525336/insomnium-the-descent.mp3'),
 (1846,8887,'Inertia','https://musify.club/track/dl/1904567/insomnium-inertia.mp3'),
 (1847,8887,'Through The Shadows','https://musify.club/track/dl/1904568/insomnium-through-the-shadows.mp3'),
 (1848,8887,'Song Of The Blackest Bird','https://musify.club/track/dl/1904569/insomnium-song-of-the-blackest-bird.mp3'),
 (1849,8887,'Only One Who Waits','https://musify.club/track/dl/1904570/insomnium-only-one-who-waits.mp3'),
 (1850,8887,'Unsung','https://musify.club/track/dl/1904571/insomnium-unsung.mp3'),
 (1851,8887,'Every Hour Wounds','https://musify.club/track/dl/1904572/insomnium-every-hour-wounds.mp3'),
 (1852,8887,'Decoherence','https://musify.club/track/dl/1904573/insomnium-decoherence.mp3'),
 (1853,8887,'Lay The Ghost To Rest','https://musify.club/track/dl/1904574/insomnium-lay-the-ghost-to-rest.mp3'),
 (1854,8887,'Regain The Fire','https://musify.club/track/dl/1904575/insomnium-regain-the-fire.mp3'),
 (1855,8887,'One For Sorrow','https://musify.club/track/dl/1904576/insomnium-one-for-sorrow.mp3'),
 (1856,8887,'Weather The Storm (feat. Mikael Stanne) [Bonus Track]','https://musify.club/track/dl/1904577/insomnium-weather-the-storm-feat-mikael-stanne-bonus-track.mp3'),
 (1857,8887,'Beyond The Horizon [Bonus Track]','https://musify.club/track/dl/6506733/insomnium-beyond-the-horizon-bonus-track.mp3'),
 (1858,8602,'Derelict','https://musify.club/track/dl/17316094/alphaxone-derelict.mp3'),
 (1859,8602,'Experience','https://musify.club/track/dl/17316095/alphaxone-experience.mp3'),
 (1860,8602,'Compression','https://musify.club/track/dl/17316096/alphaxone-compression.mp3'),
 (1861,8602,'Substance','https://musify.club/track/dl/17316097/alphaxone-substance.mp3'),
 (1862,8602,'Dissociative','https://musify.club/track/dl/17316098/alphaxone-dissociative.mp3'),
 (1863,8602,'Involvement','https://musify.club/track/dl/17316099/alphaxone-involvement.mp3'),
 (1864,8602,'Underneath','https://musify.club/track/dl/17316100/alphaxone-underneath.mp3'),
 (1865,8602,'Aftershock','https://musify.club/track/dl/17316101/alphaxone-aftershock.mp3'),
 (1866,8602,'Dissolution','https://musify.club/track/dl/17316102/alphaxone-dissolution.mp3'),
 (1867,9668,'Butterfly','https://musify.club/track/dl/10209181/mental-discipline-butterfly.mp3'),
 (1868,9668,'Make You Happy','https://musify.club/track/dl/10209182/mental-discipline-make-you-happy.mp3'),
 (1869,9668,'A Sweet Lie (Feat. 64 Rockets)','https://musify.club/track/dl/10209183/mental-discipline-a-sweet-lie-feat-64-rockets.mp3'),
 (1870,9668,'Worse (Feat. Pulcher Femina)','https://musify.club/track/dl/10209184/mental-discipline-worse-feat-pulcher-femina.mp3'),
 (1871,9668,'Lifekiller (Feat. Lights Of Euphoria)','https://musify.club/track/dl/10209185/mental-discipline-lifekiller-feat-lights-of-euphoria.mp3'),
 (1872,9668,'Time To Stay Alone','https://musify.club/track/dl/10209186/mental-discipline-time-to-stay-alone.mp3'),
 (1873,9668,'End Of Days (Feat. Namnambulu)','https://musify.club/track/dl/10209187/mental-discipline-end-of-days-feat-namnambulu.mp3'),
 (1874,9668,'Kill Emotions','https://musify.club/track/dl/10209188/mental-discipline-kill-emotions.mp3'),
 (1875,9668,'Synthetic Soul (Feat. Synapsyche)','https://musify.club/track/dl/10209189/mental-discipline-synthetic-soul-feat-synapsyche.mp3'),
 (1876,9668,'Standing Alive','https://musify.club/track/dl/10209190/mental-discipline-standing-alive.mp3'),
 (1877,9668,'Cruel (Feat. Babylonia)','https://musify.club/track/dl/10209191/mental-discipline-cruel-feat-babylonia.mp3'),
 (1878,9668,'Follow Your Star (Feat. Ashbury Heights)','https://musify.club/track/dl/10209192/mental-discipline-follow-your-star-feat-ashbury-heights.mp3'),
 (1879,9668,'Place Where I Belong','https://musify.club/track/dl/10209193/mental-discipline-place-where-i-belong.mp3'),
 (1880,9307,'Astral Fobia','https://musify.club/track/dl/8996760/centavra-project-median-project-astral-fobia.mp3'),
 (1881,9307,'In the Depths of Space','https://musify.club/track/dl/8996761/median-project-in-the-depths-of-space.mp3'),
 (1882,9307,'Heaven and Earth','https://musify.club/track/dl/8996762/median-project-heaven-and-earth.mp3'),
 (1883,9307,'In the Dark','https://musify.club/track/dl/8996763/median-project-in-the-dark.mp3'),
 (1884,9307,'Morning in Space','https://musify.club/track/dl/8996764/median-project-atlantis-greece-morning-in-space.mp3'),
 (1885,9307,'Life Support System [Median Project Remix]','https://musify.club/track/dl/8996765/artifact303-life-support-system-median-project-remix.mp3'),
 (1886,9307,'Wishmaster','https://musify.club/track/dl/8996766/median-project-wishmaster.mp3'),
 (1887,9307,'The Third Element (Original Mix)','https://musify.club/track/dl/8996767/median-project-the-third-element-original-mix.mp3'),
 (1888,9307,'Over the Horizon','https://musify.club/track/dl/8996768/median-project-over-the-horizon.mp3'),
 (1889,9307,'Комета','https://musify.club/track/dl/8996769/median-project-%d0%ba%d0%be%d0%bc%d0%b5%d1%82%d0%b0.mp3'),
 (1890,9287,'Free Choice','https://musify.club/track/dl/15474369/median-project-free-choice.mp3'),
 (1891,9287,'Behind Closed Doors','https://musify.club/track/dl/15474370/median-project-behind-closed-doors.mp3'),
 (1892,9287,'Spirit Skarma','https://musify.club/track/dl/15474371/median-project-spirit-skarma.mp3'),
 (1893,9814,'Strange Days (Neurotech Cover)','https://musify.club/track/dl/16300487/neurotech-strange-days-neurotech-cover.mp3'),
 (1894,9811,'Celldweller - Louder Than Words (Neurotech Remix)','https://musify.club/track/dl/16266220/neurotech-celldweller-louder-than-words-neurotech-remix.mp3'),
 (1895,9811,'Blue Stahli - Corner (Neurotech Remix)','https://musify.club/track/dl/16266221/neurotech-blue-stahli-corner-neurotech-remix.mp3'),
 (1896,9811,'Sybreed - Doomsday Party (Neurotech Remix)','https://musify.club/track/dl/16266222/neurotech-sybreed-doomsday-party-neurotech-remix.mp3'),
 (1897,9811,'Torul - All (Neurotech Remix)','https://musify.club/track/dl/16266223/neurotech-torul-all-neurotech-remix.mp3'),
 (1898,9811,'Torul - The Fall (Neurotech Remix)','https://musify.club/track/dl/16266224/neurotech-torul-the-fall-neurotech-remix.mp3'),
 (1899,9830,'Hope Is Better Than A Memory','https://musify.club/track/dl/16266213/neurotech-hope-is-better-than-a-memory.mp3');