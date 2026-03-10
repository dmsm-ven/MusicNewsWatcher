# DownloadHistoryApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|------------- | ------------- | -------------|
| [**apiDownloadHistoryGet**](DownloadHistoryApi.md#apidownloadhistoryget) | **GET** /api/download-history |  |
| [**apiDownloadHistoryPost**](DownloadHistoryApi.md#apidownloadhistorypost) | **POST** /api/download-history |  |



## apiDownloadHistoryGet

> Array&lt;TrackDownloadHistoryDto&gt; apiDownloadHistoryGet(limit)



### Example

```ts
import {
  Configuration,
  DownloadHistoryApi,
} from '';
import type { ApiDownloadHistoryGetRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const api = new DownloadHistoryApi();

  const body = {
    // number (optional)
    limit: 56,
  } satisfies ApiDownloadHistoryGetRequest;

  try {
    const data = await api.apiDownloadHistoryGet(body);
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
| **limit** | `number` |  | [Optional] [Defaults to `100`] |

### Return type

[**Array&lt;TrackDownloadHistoryDto&gt;**](TrackDownloadHistoryDto.md)

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


## apiDownloadHistoryPost

> apiDownloadHistoryPost(trackDownloadHistoryRequest)



### Example

```ts
import {
  Configuration,
  DownloadHistoryApi,
} from '';
import type { ApiDownloadHistoryPostRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const api = new DownloadHistoryApi();

  const body = {
    // TrackDownloadHistoryRequest (optional)
    trackDownloadHistoryRequest: ...,
  } satisfies ApiDownloadHistoryPostRequest;

  try {
    const data = await api.apiDownloadHistoryPost(body);
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
| **trackDownloadHistoryRequest** | [TrackDownloadHistoryRequest](TrackDownloadHistoryRequest.md) |  | [Optional] |

### Return type

`void` (Empty response body)

### Authorization

No authorization required

### HTTP request headers

- **Content-Type**: `application/json`, `text/json`, `application/*+json`
- **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)

