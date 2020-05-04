/* tslint:disable */
/* eslint-disable */
//  This file was automatically generated and should not be edited.

export type CreateAlbumInput = {
  id?: string | null,
  name: string,
};

export type ModelAlbumConditionInput = {
  name?: ModelStringInput | null,
  and?: Array< ModelAlbumConditionInput | null > | null,
  or?: Array< ModelAlbumConditionInput | null > | null,
  not?: ModelAlbumConditionInput | null,
};

export type ModelStringInput = {
  ne?: string | null,
  eq?: string | null,
  le?: string | null,
  lt?: string | null,
  ge?: string | null,
  gt?: string | null,
  contains?: string | null,
  notContains?: string | null,
  between?: Array< string | null > | null,
  beginsWith?: string | null,
  attributeExists?: boolean | null,
  attributeType?: ModelAttributeTypes | null,
  size?: ModelSizeInput | null,
};

export enum ModelAttributeTypes {
  binary = "binary",
  binarySet = "binarySet",
  bool = "bool",
  list = "list",
  map = "map",
  number = "number",
  numberSet = "numberSet",
  string = "string",
  stringSet = "stringSet",
  _null = "_null",
}


export type ModelSizeInput = {
  ne?: number | null,
  eq?: number | null,
  le?: number | null,
  lt?: number | null,
  ge?: number | null,
  gt?: number | null,
  between?: Array< number | null > | null,
};

export type UpdateAlbumInput = {
  id: string,
  name?: string | null,
};

export type DeleteAlbumInput = {
  id?: string | null,
};

export type CreateVideoInput = {
  id?: string | null,
  owner?: string | null,
  albumId: string,
  bucket: string,
  fullsize: VideoS3InfoInput,
  thumbnail: VideoS3InfoInput,
  labels?: Array< string | null > | null,
};

export type VideoS3InfoInput = {
  key: string,
  width: number,
  height: number,
};

export type ModelVideoConditionInput = {
  albumId?: ModelIDInput | null,
  bucket?: ModelStringInput | null,
  labels?: ModelStringInput | null,
  and?: Array< ModelVideoConditionInput | null > | null,
  or?: Array< ModelVideoConditionInput | null > | null,
  not?: ModelVideoConditionInput | null,
};

export type ModelIDInput = {
  ne?: string | null,
  eq?: string | null,
  le?: string | null,
  lt?: string | null,
  ge?: string | null,
  gt?: string | null,
  contains?: string | null,
  notContains?: string | null,
  between?: Array< string | null > | null,
  beginsWith?: string | null,
  attributeExists?: boolean | null,
  attributeType?: ModelAttributeTypes | null,
  size?: ModelSizeInput | null,
};

export type UpdateVideoInput = {
  id: string,
  albumId?: string | null,
  bucket?: string | null,
  fullsize?: VideoS3InfoInput | null,
  thumbnail?: VideoS3InfoInput | null,
  labels?: Array< string | null > | null,
};

export type DeleteVideoInput = {
  id?: string | null,
};

export type CreateVocabularyInput = {
  id?: string | null,
  userId: string,
  vocabularies: Array< string >,
};

export type ModelVocabularyConditionInput = {
  userId?: ModelStringInput | null,
  vocabularies?: ModelStringInput | null,
  and?: Array< ModelVocabularyConditionInput | null > | null,
  or?: Array< ModelVocabularyConditionInput | null > | null,
  not?: ModelVocabularyConditionInput | null,
};

export type UpdateVocabularyInput = {
  id: string,
  userId?: string | null,
  vocabularies?: Array< string > | null,
};

export type DeleteVocabularyInput = {
  id?: string | null,
};

export type ModelAlbumFilterInput = {
  id?: ModelIDInput | null,
  name?: ModelStringInput | null,
  and?: Array< ModelAlbumFilterInput | null > | null,
  or?: Array< ModelAlbumFilterInput | null > | null,
  not?: ModelAlbumFilterInput | null,
};

export type ModelVideoFilterInput = {
  id?: ModelIDInput | null,
  albumId?: ModelIDInput | null,
  bucket?: ModelStringInput | null,
  labels?: ModelStringInput | null,
  and?: Array< ModelVideoFilterInput | null > | null,
  or?: Array< ModelVideoFilterInput | null > | null,
  not?: ModelVideoFilterInput | null,
};

export type ModelVocabularyFilterInput = {
  id?: ModelIDInput | null,
  userId?: ModelStringInput | null,
  vocabularies?: ModelStringInput | null,
  and?: Array< ModelVocabularyFilterInput | null > | null,
  or?: Array< ModelVocabularyFilterInput | null > | null,
  not?: ModelVocabularyFilterInput | null,
};

export enum ModelSortDirection {
  ASC = "ASC",
  DESC = "DESC",
}


export type CreateAlbumMutationVariables = {
  input: CreateAlbumInput,
  condition?: ModelAlbumConditionInput | null,
};

export type CreateAlbumMutation = {
  createAlbum:  {
    __typename: "Album",
    id: string,
    name: string,
    Videos:  {
      __typename: "ModelVideoConnection",
      items:  Array< {
        __typename: "Video",
        id: string,
        albumId: string,
        bucket: string,
        labels: Array< string | null > | null,
      } | null > | null,
      nextToken: string | null,
    } | null,
  } | null,
};

export type UpdateAlbumMutationVariables = {
  input: UpdateAlbumInput,
  condition?: ModelAlbumConditionInput | null,
};

export type UpdateAlbumMutation = {
  updateAlbum:  {
    __typename: "Album",
    id: string,
    name: string,
    Videos:  {
      __typename: "ModelVideoConnection",
      items:  Array< {
        __typename: "Video",
        id: string,
        albumId: string,
        bucket: string,
        labels: Array< string | null > | null,
      } | null > | null,
      nextToken: string | null,
    } | null,
  } | null,
};

export type DeleteAlbumMutationVariables = {
  input: DeleteAlbumInput,
  condition?: ModelAlbumConditionInput | null,
};

export type DeleteAlbumMutation = {
  deleteAlbum:  {
    __typename: "Album",
    id: string,
    name: string,
    Videos:  {
      __typename: "ModelVideoConnection",
      items:  Array< {
        __typename: "Video",
        id: string,
        albumId: string,
        bucket: string,
        labels: Array< string | null > | null,
      } | null > | null,
      nextToken: string | null,
    } | null,
  } | null,
};

export type CreateVideoMutationVariables = {
  input: CreateVideoInput,
  condition?: ModelVideoConditionInput | null,
};

export type CreateVideoMutation = {
  createVideo:  {
    __typename: "Video",
    id: string,
    albumId: string,
    album:  {
      __typename: "Album",
      id: string,
      name: string,
      Videos:  {
        __typename: "ModelVideoConnection",
        nextToken: string | null,
      } | null,
    } | null,
    bucket: string,
    fullsize:  {
      __typename: "VideoS3Info",
      key: string,
      width: number,
      height: number,
    },
    thumbnail:  {
      __typename: "VideoS3Info",
      key: string,
      width: number,
      height: number,
    },
    labels: Array< string | null > | null,
  } | null,
};

export type UpdateVideoMutationVariables = {
  input: UpdateVideoInput,
  condition?: ModelVideoConditionInput | null,
};

export type UpdateVideoMutation = {
  updateVideo:  {
    __typename: "Video",
    id: string,
    albumId: string,
    album:  {
      __typename: "Album",
      id: string,
      name: string,
      Videos:  {
        __typename: "ModelVideoConnection",
        nextToken: string | null,
      } | null,
    } | null,
    bucket: string,
    fullsize:  {
      __typename: "VideoS3Info",
      key: string,
      width: number,
      height: number,
    },
    thumbnail:  {
      __typename: "VideoS3Info",
      key: string,
      width: number,
      height: number,
    },
    labels: Array< string | null > | null,
  } | null,
};

export type DeleteVideoMutationVariables = {
  input: DeleteVideoInput,
  condition?: ModelVideoConditionInput | null,
};

export type DeleteVideoMutation = {
  deleteVideo:  {
    __typename: "Video",
    id: string,
    albumId: string,
    album:  {
      __typename: "Album",
      id: string,
      name: string,
      Videos:  {
        __typename: "ModelVideoConnection",
        nextToken: string | null,
      } | null,
    } | null,
    bucket: string,
    fullsize:  {
      __typename: "VideoS3Info",
      key: string,
      width: number,
      height: number,
    },
    thumbnail:  {
      __typename: "VideoS3Info",
      key: string,
      width: number,
      height: number,
    },
    labels: Array< string | null > | null,
  } | null,
};

export type CreateVocabularyMutationVariables = {
  input: CreateVocabularyInput,
  condition?: ModelVocabularyConditionInput | null,
};

export type CreateVocabularyMutation = {
  createVocabulary:  {
    __typename: "Vocabulary",
    id: string,
    userId: string,
    vocabularies: Array< string >,
    owner: string | null,
  } | null,
};

export type UpdateVocabularyMutationVariables = {
  input: UpdateVocabularyInput,
  condition?: ModelVocabularyConditionInput | null,
};

export type UpdateVocabularyMutation = {
  updateVocabulary:  {
    __typename: "Vocabulary",
    id: string,
    userId: string,
    vocabularies: Array< string >,
    owner: string | null,
  } | null,
};

export type DeleteVocabularyMutationVariables = {
  input: DeleteVocabularyInput,
  condition?: ModelVocabularyConditionInput | null,
};

export type DeleteVocabularyMutation = {
  deleteVocabulary:  {
    __typename: "Vocabulary",
    id: string,
    userId: string,
    vocabularies: Array< string >,
    owner: string | null,
  } | null,
};

export type GetAlbumQueryVariables = {
  id: string,
};

export type GetAlbumQuery = {
  getAlbum:  {
    __typename: "Album",
    id: string,
    name: string,
    Videos:  {
      __typename: "ModelVideoConnection",
      items:  Array< {
        __typename: "Video",
        id: string,
        albumId: string,
        bucket: string,
        labels: Array< string | null > | null,
      } | null > | null,
      nextToken: string | null,
    } | null,
  } | null,
};

export type ListAlbumsQueryVariables = {
  filter?: ModelAlbumFilterInput | null,
  limit?: number | null,
  nextToken?: string | null,
};

export type ListAlbumsQuery = {
  listAlbums:  {
    __typename: "ModelAlbumConnection",
    items:  Array< {
      __typename: "Album",
      id: string,
      name: string,
      Videos:  {
        __typename: "ModelVideoConnection",
        nextToken: string | null,
      } | null,
    } | null > | null,
    nextToken: string | null,
  } | null,
};

export type GetVideoQueryVariables = {
  id: string,
};

export type GetVideoQuery = {
  getVideo:  {
    __typename: "Video",
    id: string,
    albumId: string,
    album:  {
      __typename: "Album",
      id: string,
      name: string,
      Videos:  {
        __typename: "ModelVideoConnection",
        nextToken: string | null,
      } | null,
    } | null,
    bucket: string,
    fullsize:  {
      __typename: "VideoS3Info",
      key: string,
      width: number,
      height: number,
    },
    thumbnail:  {
      __typename: "VideoS3Info",
      key: string,
      width: number,
      height: number,
    },
    labels: Array< string | null > | null,
  } | null,
};

export type ListVideosQueryVariables = {
  filter?: ModelVideoFilterInput | null,
  limit?: number | null,
  nextToken?: string | null,
};

export type ListVideosQuery = {
  listVideos:  {
    __typename: "ModelVideoConnection",
    items:  Array< {
      __typename: "Video",
      id: string,
      albumId: string,
      album:  {
        __typename: "Album",
        id: string,
        name: string,
      } | null,
      bucket: string,
      fullsize:  {
        __typename: "VideoS3Info",
        key: string,
        width: number,
        height: number,
      },
      thumbnail:  {
        __typename: "VideoS3Info",
        key: string,
        width: number,
        height: number,
      },
      labels: Array< string | null > | null,
    } | null > | null,
    nextToken: string | null,
  } | null,
};

export type GetVocabularyQueryVariables = {
  id: string,
};

export type GetVocabularyQuery = {
  getVocabulary:  {
    __typename: "Vocabulary",
    id: string,
    userId: string,
    vocabularies: Array< string >,
    owner: string | null,
  } | null,
};

export type ListVocabularysQueryVariables = {
  filter?: ModelVocabularyFilterInput | null,
  limit?: number | null,
  nextToken?: string | null,
};

export type ListVocabularysQuery = {
  listVocabularys:  {
    __typename: "ModelVocabularyConnection",
    items:  Array< {
      __typename: "Vocabulary",
      id: string,
      userId: string,
      vocabularies: Array< string >,
      owner: string | null,
    } | null > | null,
    nextToken: string | null,
  } | null,
};

export type ListVideosByAlbumQueryVariables = {
  albumId?: string | null,
  sortDirection?: ModelSortDirection | null,
  filter?: ModelVideoFilterInput | null,
  limit?: number | null,
  nextToken?: string | null,
};

export type ListVideosByAlbumQuery = {
  listVideosByAlbum:  {
    __typename: "ModelVideoConnection",
    items:  Array< {
      __typename: "Video",
      id: string,
      albumId: string,
      album:  {
        __typename: "Album",
        id: string,
        name: string,
      } | null,
      bucket: string,
      fullsize:  {
        __typename: "VideoS3Info",
        key: string,
        width: number,
        height: number,
      },
      thumbnail:  {
        __typename: "VideoS3Info",
        key: string,
        width: number,
        height: number,
      },
      labels: Array< string | null > | null,
    } | null > | null,
    nextToken: string | null,
  } | null,
};

export type OnCreateAlbumSubscription = {
  onCreateAlbum:  {
    __typename: "Album",
    id: string,
    name: string,
    Videos:  {
      __typename: "ModelVideoConnection",
      items:  Array< {
        __typename: "Video",
        id: string,
        albumId: string,
        bucket: string,
        labels: Array< string | null > | null,
      } | null > | null,
      nextToken: string | null,
    } | null,
  } | null,
};

export type OnUpdateAlbumSubscription = {
  onUpdateAlbum:  {
    __typename: "Album",
    id: string,
    name: string,
    Videos:  {
      __typename: "ModelVideoConnection",
      items:  Array< {
        __typename: "Video",
        id: string,
        albumId: string,
        bucket: string,
        labels: Array< string | null > | null,
      } | null > | null,
      nextToken: string | null,
    } | null,
  } | null,
};

export type OnDeleteAlbumSubscription = {
  onDeleteAlbum:  {
    __typename: "Album",
    id: string,
    name: string,
    Videos:  {
      __typename: "ModelVideoConnection",
      items:  Array< {
        __typename: "Video",
        id: string,
        albumId: string,
        bucket: string,
        labels: Array< string | null > | null,
      } | null > | null,
      nextToken: string | null,
    } | null,
  } | null,
};

export type OnCreateVideoSubscription = {
  onCreateVideo:  {
    __typename: "Video",
    id: string,
    albumId: string,
    album:  {
      __typename: "Album",
      id: string,
      name: string,
      Videos:  {
        __typename: "ModelVideoConnection",
        nextToken: string | null,
      } | null,
    } | null,
    bucket: string,
    fullsize:  {
      __typename: "VideoS3Info",
      key: string,
      width: number,
      height: number,
    },
    thumbnail:  {
      __typename: "VideoS3Info",
      key: string,
      width: number,
      height: number,
    },
    labels: Array< string | null > | null,
  } | null,
};

export type OnUpdateVideoSubscription = {
  onUpdateVideo:  {
    __typename: "Video",
    id: string,
    albumId: string,
    album:  {
      __typename: "Album",
      id: string,
      name: string,
      Videos:  {
        __typename: "ModelVideoConnection",
        nextToken: string | null,
      } | null,
    } | null,
    bucket: string,
    fullsize:  {
      __typename: "VideoS3Info",
      key: string,
      width: number,
      height: number,
    },
    thumbnail:  {
      __typename: "VideoS3Info",
      key: string,
      width: number,
      height: number,
    },
    labels: Array< string | null > | null,
  } | null,
};

export type OnDeleteVideoSubscription = {
  onDeleteVideo:  {
    __typename: "Video",
    id: string,
    albumId: string,
    album:  {
      __typename: "Album",
      id: string,
      name: string,
      Videos:  {
        __typename: "ModelVideoConnection",
        nextToken: string | null,
      } | null,
    } | null,
    bucket: string,
    fullsize:  {
      __typename: "VideoS3Info",
      key: string,
      width: number,
      height: number,
    },
    thumbnail:  {
      __typename: "VideoS3Info",
      key: string,
      width: number,
      height: number,
    },
    labels: Array< string | null > | null,
  } | null,
};

export type OnCreateVocabularySubscriptionVariables = {
  owner: string,
};

export type OnCreateVocabularySubscription = {
  onCreateVocabulary:  {
    __typename: "Vocabulary",
    id: string,
    userId: string,
    vocabularies: Array< string >,
    owner: string | null,
  } | null,
};

export type OnUpdateVocabularySubscriptionVariables = {
  owner: string,
};

export type OnUpdateVocabularySubscription = {
  onUpdateVocabulary:  {
    __typename: "Vocabulary",
    id: string,
    userId: string,
    vocabularies: Array< string >,
    owner: string | null,
  } | null,
};

export type OnDeleteVocabularySubscriptionVariables = {
  owner: string,
};

export type OnDeleteVocabularySubscription = {
  onDeleteVocabulary:  {
    __typename: "Vocabulary",
    id: string,
    userId: string,
    vocabularies: Array< string >,
    owner: string | null,
  } | null,
};
