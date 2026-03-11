# MusicNewsWatcherApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|------------- | ------------- | -------------|
| [**apiAlbumsAlbumIdProviderProviderIdGet**](MusicNewsWatcherApi.md#apialbumsalbumidproviderprovideridget) | **GET** /api/albums/{album_id}/provider/{provider_id} |  |
| [**apiArtistsArtistIdAlbumsGet**](MusicNewsWatcherApi.md#apiartistsartistidalbumsget) | **GET** /api/artists/{artist_id}/albums |  |
| [**apiArtistsArtistIdDelete**](MusicNewsWatcherApi.md#apiartistsartistiddelete) | **DELETE** /api/artists/{artist_id} |  |
| [**apiArtistsArtistIdPut**](MusicNewsWatcherApi.md#apiartistsartistidput) | **PUT** /api/artists/{artist_id} |  |
| [**apiArtistsArtistIdRefreshAlbumsPost**](MusicNewsWatcherApi.md#apiartistsartistidrefreshalbumspost) | **POST** /api/artists/{artist_id}/refresh-albums |  |
| [**apiArtistsPost**](MusicNewsWatcherApi.md#apiartistspost) | **POST** /api/artists |  |
| [**apiProvidersGet**](MusicNewsWatcherApi.md#apiprovidersget) | **GET** /api/providers |  |
| [**apiProvidersProviderIdArtistsGet**](MusicNewsWatcherApi.md#apiprovidersprovideridartistsget) | **GET** /api/providers/{provider_id}/artists |  |
| [**apiStatusGet**](MusicNewsWatcherApi.md#apistatusget) | **GET** /api/status |  |



## apiAlbumsAlbumIdProviderProviderIdGet

> Array&lt;AlbumDto&gt; apiAlbumsAlbumIdProviderProviderIdGet(albumId, providerId)



### Example

```ts
import {
  Configuration,
  MusicNewsWatcherApi,
} from '';
import type { ApiAlbumsAlbumIdProviderProviderIdGetRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const api = new MusicNewsWatcherApi();

  const body = {
    // number
    albumId: 56,
    // number
    providerId: 56,
  } satisfies ApiAlbumsAlbumIdProviderProviderIdGetRequest;

  try {
    const data = await api.apiAlbumsAlbumIdProviderProviderIdGet(body);
    console.log(data);
  } catch (error) {
    console.error(error);
  }
}

// Run the test
example().catch(console.error);
```

### Parameters


| Name | Type | Description  | Notes |
|------------- | ------------- | ------------- | -------------|
| **albumId** | `number` |  | [Defaults to `undefined`] |
| **providerId** | `number` |  | [Defaults to `undefined`] |

### Return type

[**Array&lt;AlbumDto&gt;**](AlbumDto.md)

### Authorization

No authorization required

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: `text/plain`, `application/json`, `text/json`


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


## apiArtistsArtistIdAlbumsGet

> Array&lt;AlbumDto&gt; apiArtistsArtistIdAlbumsGet(artistId)



### Example

```ts
import {
  Configuration,
  MusicNewsWatcherApi,
} from '';
import type { ApiArtistsArtistIdAlbumsGetRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const api = new MusicNewsWatcherApi();

  const body = {
    // number
    artistId: 56,
  } satisfies ApiArtistsArtistIdAlbumsGetRequest;

  try {
    const data = await api.apiArtistsArtistIdAlbumsGet(body);
    console.log(data);
  } catch (error) {
    console.error(error);
  }
}

// Run the test
example().catch(console.error);
```

### Parameters


| Name | Type | Description  | Notes |
|------------- | ------------- | ------------- | -------------|
| **artistId** | `number` |  | [Defaults to `undefined`] |

### Return type

[**Array&lt;AlbumDto&gt;**](AlbumDto.md)

### Authorization

No authorization required

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: `text/plain`, `application/json`, `text/json`


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


## apiArtistsArtistIdDelete

> apiArtistsArtistIdDelete(artistId)



### Example

```ts
import {
  Configuration,
  MusicNewsWatcherApi,
} from '';
import type { ApiArtistsArtistIdDeleteRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const api = new MusicNewsWatcherApi();

  const body = {
    // number
    artistId: 56,
  } satisfies ApiArtistsArtistIdDeleteRequest;

  try {
    const data = await api.apiArtistsArtistIdDelete(body);
    console.log(data);
  } catch (error) {
    console.error(error);
  }
}

// Run the test
example().catch(console.error);
```

### Parameters


| Name | Type | Description  | Notes |
|------------- | ------------- | ------------- | -------------|
| **artistId** | `number` |  | [Defaults to `undefined`] |

### Return type

`void` (Empty response body)

### Authorization

No authorization required

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


## apiArtistsArtistIdPut

> ArtistDto apiArtistsArtistIdPut(artistId, artistDto)



### Example

```ts
import {
  Configuration,
  MusicNewsWatcherApi,
} from '';
import type { ApiArtistsArtistIdPutRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const api = new MusicNewsWatcherApi();

  const body = {
    // number
    artistId: 56,
    // ArtistDto (optional)
    artistDto: ...,
  } satisfies ApiArtistsArtistIdPutRequest;

  try {
    const data = await api.apiArtistsArtistIdPut(body);
    console.log(data);
  } catch (error) {
    console.error(error);
  }
}

// Run the test
example().catch(console.error);
```

### Parameters


| Name | Type | Description  | Notes |
|------------- | ------------- | ------------- | -------------|
| **artistId** | `number` |  | [Defaults to `undefined`] |
| **artistDto** | [ArtistDto](ArtistDto.md) |  | [Optional] |

### Return type

[**ArtistDto**](ArtistDto.md)

### Authorization

No authorization required

### HTTP request headers

- **Content-Type**: `application/json`, `text/json`, `application/*+json`
- **Accept**: `text/plain`, `application/json`, `text/json`


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


## apiArtistsArtistIdRefreshAlbumsPost

> apiArtistsArtistIdRefreshAlbumsPost(artistId)



### Example

```ts
import {
  Configuration,
  MusicNewsWatcherApi,
} from '';
import type { ApiArtistsArtistIdRefreshAlbumsPostRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const api = new MusicNewsWatcherApi();

  const body = {
    // number
    artistId: 56,
  } satisfies ApiArtistsArtistIdRefreshAlbumsPostRequest;

  try {
    const data = await api.apiArtistsArtistIdRefreshAlbumsPost(body);
    console.log(data);
  } catch (error) {
    console.error(error);
  }
}

// Run the test
example().catch(console.error);
```

### Parameters


| Name | Type | Description  | Notes |
|------------- | ------------- | ------------- | -------------|
| **artistId** | `number` |  | [Defaults to `undefined`] |

### Return type

`void` (Empty response body)

### Authorization

No authorization required

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


## apiArtistsPost

> ArtistDto apiArtistsPost(createArtistDto)



### Example

```ts
import {
  Configuration,
  MusicNewsWatcherApi,
} from '';
import type { ApiArtistsPostRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const api = new MusicNewsWatcherApi();

  const body = {
    // CreateArtistDto (optional)
    createArtistDto: ...,
  } satisfies ApiArtistsPostRequest;

  try {
    const data = await api.apiArtistsPost(body);
    console.log(data);
  } catch (error) {
    console.error(error);
  }
}

// Run the test
example().catch(console.error);
```

### Parameters


| Name | Type | Description  | Notes |
|------------- | ------------- | ------------- | -------------|
| **createArtistDto** | [CreateArtistDto](CreateArtistDto.md) |  | [Optional] |

### Return type

[**ArtistDto**](ArtistDto.md)

### Authorization

No authorization required

### HTTP request headers

- **Content-Type**: `application/json`, `text/json`, `application/*+json`
- **Accept**: `text/plain`, `application/json`, `text/json`


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


## apiProvidersGet

> Array&lt;MusicProviderDto&gt; apiProvidersGet()



### Example

```ts
import {
  Configuration,
  MusicNewsWatcherApi,
} from '';
import type { ApiProvidersGetRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const api = new MusicNewsWatcherApi();

  try {
    const data = await api.apiProvidersGet();
    console.log(data);
  } catch (error) {
    console.error(error);
  }
}

// Run the test
example().catch(console.error);
```

### Parameters

This endpoint does not need any parameter.

### Return type

[**Array&lt;MusicProviderDto&gt;**](MusicProviderDto.md)

### Authorization

No authorization required

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: `text/plain`, `application/json`, `text/json`


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


## apiProvidersProviderIdArtistsGet

> Array&lt;ArtistDto&gt; apiProvidersProviderIdArtistsGet(providerId)



### Example

```ts
import {
  Configuration,
  MusicNewsWatcherApi,
} from '';
import type { ApiProvidersProviderIdArtistsGetRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const api = new MusicNewsWatcherApi();

  const body = {
    // number
    providerId: 56,
  } satisfies ApiProvidersProviderIdArtistsGetRequest;

  try {
    const data = await api.apiProvidersProviderIdArtistsGet(body);
    console.log(data);
  } catch (error) {
    console.error(error);
  }
}

// Run the test
example().catch(console.error);
```

### Parameters


| Name | Type | Description  | Notes |
|------------- | ------------- | ------------- | -------------|
| **providerId** | `number` |  | [Defaults to `undefined`] |

### Return type

[**Array&lt;ArtistDto&gt;**](ArtistDto.md)

### Authorization

No authorization required

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: `text/plain`, `application/json`, `text/json`


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


## apiStatusGet

> apiStatusGet()



### Example

```ts
import {
  Configuration,
  MusicNewsWatcherApi,
} from '';
import type { ApiStatusGetRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const api = new MusicNewsWatcherApi();

  try {
    const data = await api.apiStatusGet();
    console.log(data);
  } catch (error) {
    console.error(error);
  }
}

// Run the test
example().catch(console.error);
```

### Parameters

This endpoint does not need any parameter.

### Return type

`void` (Empty response body)

### Authorization

No authorization required

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)

