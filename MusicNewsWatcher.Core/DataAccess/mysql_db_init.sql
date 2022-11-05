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