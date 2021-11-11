import React, { useState } from 'react';
import { Button, Form, FormGroup, Label, Input, FormText ,ListGroup, ListGroupItem} from 'reactstrap';

const FileUpload = (props) =>
{
    const [uploadedFiles, setUploadedFiles] = useState([]);

    const onFileUploaded = (evt) =>
    {
        var currFiles = Array.from(evt.currentTarget.files);
        console.log(currFiles);
        setUploadedFiles(currFiles);       
        props.onImageLoad(currFiles);
    }

    return (<div>
        <Form>
            <FormGroup>
                <Label for="FilesToUpload">Upload the images to analyze</Label>
                <p><input type="file" id="files" multiple={true} onChange={onFileUploaded}/></p>
            </FormGroup>
            <FormGroup>
                <Label for="UploadedFiles">Uploaded Images</Label>
                <div>
                    <ListGroup>
                        {
                            uploadedFiles.map((file) => {
                                return <ListGroupItem>{ file.name }</ListGroupItem>
                            })
                        }
                    </ListGroup>
                </div>
            </FormGroup>
        </Form>
    </div>)
}

export default FileUpload;