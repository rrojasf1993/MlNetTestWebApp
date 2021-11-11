import React, { useState, useReducer } from 'react';
import { Jumbotron, Button, Fade } from 'reactstrap';
import FileUpload from '../FileUploader/FileUpload';
import Swal from 'sweetalert2';
import Loader from 'react-loader-spinner';
import "react-loader-spinner/dist/loader/css/react-spinner-loader.css";
import { uploadFiles, analyzeUploadedPhotos } from '../../services/MLImageAnalysisService';
import ImageCard from '../ImagePresenter/ImageCard';
import ReactLoading from "react-loading";

const ImagesAnalysis = () => {


    const [loadedImages, setLoadedImages] = useState([]);
    const [blockUpload, setBlockUpload] = useState(false);
    const [isAsyncCallInExecution, setIsAsyncCallInExecution] = useState(false);
    const [analyzedImageData, setAnalyzedImageData]=useState([])
   

    const onImageLoaded = (images) => {
        setLoadedImages(images);
        console.log('Loaded imgages', images);
    }

    const uploadImagesForAnalysis = async () => {
        const formData = new FormData();
        loadedImages.forEach((imgFile) => {
            formData.append('files', imgFile, imgFile.name);
        });

        setIsAsyncCallInExecution(!isAsyncCallInExecution)
        setBlockUpload(!setBlockUpload);

        var uploadRequest = await uploadFiles(formData);
        if (uploadRequest.ok)
        {
            await Swal.fire('Success', 'Files have been uploaded succesfully', 'success').then((dialogResult) =>
            {
                if (dialogResult.value === true) {
                    analyzePhotos();
                }
            });
        }
        else {
            Swal.fire('Error', 'An error has occurred, try again later', 'error');
            console.error(uploadRequest.statusText);
            setIsAsyncCallInExecution(!isAsyncCallInExecution)
            setBlockUpload(!setBlockUpload);
        }
    }

    const analyzePhotos = async () =>
    {
        const analyisisRequest = await analyzeUploadedPhotos();
        if (analyisisRequest.ok)
        {
            var result = await analyisisRequest.json();
            if (result !== null)
            {
                setIsAsyncCallInExecution(false);
                setAnalyzedImageData(result);
            }
        }
        else
        {
            Swal.fire('Error', 'An error has occurred, try again later', 'error');
            console.error(analyisisRequest.statusText);
            setIsAsyncCallInExecution(!isAsyncCallInExecution)
            setBlockUpload(!setBlockUpload);
        }
    }

    const renderComponentBasedInAsyncCall = () => {
        let componentToRender = null;
        if (isAsyncCallInExecution) {
            componentToRender = <ReactLoading type={"bars"} color="blue" />
        }
        else {
            componentToRender = (<div>
                <div>
                    <div>
                        <div>
                            <FileUpload onImageLoad={onImageLoaded} />
                        </div>
                        <div>
                            <Button color="primary"
                                onClick={uploadImagesForAnalysis}
                                disabled={blockUpload}> Upload images for analysis</Button>
                        </div>
                    </div>
                </div>
                <div>
                    {
                        analyzedImageData.map((data, index) => {
                            return (<div>
                                <ImageCard imageData={data} key={index} />
                            </div>)
                        })
                    }
                </div>
            </div>)
        }
        return componentToRender;
    }

    return (<div>
        <Jumbotron>
            <h1 className="display-3">Object detection in images using ML.NET and React.js</h1>
            <p className="lead">Upload images in the control below to analyze them</p>
        </Jumbotron>
        {
            renderComponentBasedInAsyncCall()
        }
    </div>);
}

export default ImagesAnalysis;